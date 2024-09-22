using UnityEngine;
using System.Collections;

	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class PathDrawer : MonoBehaviour 
	{
		
		protected PathSection[] sections;
		protected int maxSections = 1024;
		protected int sectionIndex = 0;
		
		protected float minDistance = 0.5f;
		protected float sectionWidth = 3.0f;
		
		protected bool updated = false;
		
		// Use this for initialization
		void Start () 
		{
			sections = new PathSection[ maxSections ];
			
			Clear();
			
			if( GetComponent<MeshFilter>().mesh == null )
				GetComponent<MeshFilter>().mesh = new Mesh();
		}
		
		public void Clear()
		{
			for(int i = 0; i < maxSections; ++i) {
				sections[i] = new PathSection();
			}
			
			sectionIndex = 0;
			updated = true;
		
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			mesh.Clear();
		}
		
		public void AddSection(Vector3 position)
		{
			// only add the section if it is far away from the previous one
			if( sectionIndex != 0 )
			{
				if( Vector3.Distance( position, sections[ sectionIndex - 1 ].pos) < minDistance )
					return;
			}
		
		
			PathSection current = sections[ sectionIndex ];
		
			//Debug.Log("ADD SECTION : " + position);
			
			current.pos = position;
			current.normal = new Vector3(0, 0.0f, -1.0f);
			current.intensity = 0.8f;			
			
			if( sectionIndex != 0 ) // if it's the first section, there is no previous one!
			{
				PathSection previous = sections[ sectionIndex - 1 ];
				Vector3 dir = current.pos - previous.pos;
				Vector3 xDir = Vector3.Cross( dir, current.normal ).normalized;
				
				current.posl = current.pos + xDir * sectionWidth * 0.5f;
				current.posr = current.pos - xDir * sectionWidth * 0.5f;
				current.tangent = new Vector4( xDir.x, xDir.y, xDir.z, 1);
				
				if( sectionIndex - 1 == 0 ) // previous is the first section
				{
					previous.tangent = current.tangent;
					previous.posl = current.pos + xDir * sectionWidth * 0.5f;
					previous.posr = current.pos - xDir * sectionWidth * 0.5f;
				}
			}
			
			
			sectionIndex++;
			updated = true;
			
			if( sectionIndex == maxSections )
			{
				Debug.LogError("TOO MANY SECTIONS!");
				sectionIndex = 0;
			}
		
			Update ();
		}
		
		// Update is called once per frame
		void Update () 
		{
			if( !updated ) {
				return;
			}
			
			updated = false;
			
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			mesh.Clear();
			
			int segmentCount = sectionIndex + 1;
			
			Vector3[] 	vertices 	= new Vector3[ 	segmentCount * 4 ];
			Vector3[] 	normals 	= new Vector3[ 	segmentCount * 4 ];
			Vector4[] 	tangents	= new Vector4[ 	segmentCount * 4 ];
			Color[] 	colors 		= new Color[ 	segmentCount * 4 ];
			Vector2[] 	uvs 		= new Vector2[ 	segmentCount * 4 ];
			int[] 		triangles 	= new int[ 		segmentCount * 6 ];
		
	
			segmentCount = 0;
			
			for( int i = 1; i < sectionIndex; ++i )
			{
				PathSection curr = sections[ i ];
				PathSection last = sections[ i - 1 ]; // previous
			
				
				vertices[segmentCount * 4 + 0] = last.posl;
				vertices[segmentCount * 4 + 1] = last.posr;
				vertices[segmentCount * 4 + 2] = curr.posl;
				vertices[segmentCount * 4 + 3] = curr.posr;
				
				normals[segmentCount * 4 + 0] = last.normal;
				normals[segmentCount * 4 + 1] = last.normal;
				normals[segmentCount * 4 + 2] = curr.normal;
				normals[segmentCount * 4 + 3] = curr.normal;
	
				tangents[segmentCount * 4 + 0] = last.tangent;
				tangents[segmentCount * 4 + 1] = last.tangent;
				tangents[segmentCount * 4 + 2] = curr.tangent;
				tangents[segmentCount * 4 + 3] = curr.tangent;
				
				colors[segmentCount * 4 + 0]=new Color(1, 1, 1, last.intensity);
				colors[segmentCount * 4 + 1]=new Color(1, 1, 1, last.intensity);
				colors[segmentCount * 4 + 2]=new Color(1, 1, 1, curr.intensity);
				colors[segmentCount * 4 + 3]=new Color(1, 1, 1, curr.intensity);
				/*
				 * // @TODO: provide another texture-part for the start and finish (nice and round, instead of hard cutoffs like now)
				if( i == 1 || i == sectionIndex - 1)
				{
					uvs[segmentCount * 4 + 0] = new Vector2(0, 0);
					uvs[segmentCount * 4 + 1] = new Vector2(0, 1);
					uvs[segmentCount * 4 + 2] = new Vector2(1, 0);
					uvs[segmentCount * 4 + 3] = new Vector2(1, 1);
				}
				else
				{
				*/
					uvs[segmentCount * 4 + 0] = new Vector2(0, 0);
					uvs[segmentCount * 4 + 1] = new Vector2(1, 0);
					uvs[segmentCount * 4 + 2] = new Vector2(0, 1);
					uvs[segmentCount * 4 + 3] = new Vector2(1, 1);
				//}
				
				triangles[segmentCount * 6 + 0] = segmentCount * 4 + 0;
				triangles[segmentCount * 6 + 2] = segmentCount * 4 + 1;
				triangles[segmentCount * 6 + 1] = segmentCount * 4 + 2;
				
				triangles[segmentCount * 6 + 3] = segmentCount * 4 + 2;
				triangles[segmentCount * 6 + 5] = segmentCount * 4 + 1;
				triangles[segmentCount * 6 + 4] = segmentCount * 4 + 3;
				segmentCount++;			
			}
			mesh.vertices  = vertices;
			mesh.normals   = normals;
			mesh.tangents  = tangents;
			mesh.triangles = triangles;
			mesh.colors    = colors;
			mesh.uv        = uvs;
		}
		
		protected class PathSection
		{
			public Vector3 pos = Vector3.zero;
			public Vector3 normal = Vector3.zero;
			public Vector4 tangent = Vector4.zero;
			public Vector3 posl = Vector3.zero;
			public Vector3 posr = Vector3.zero;
			public float intensity = 0.0f;
			//public int lastIndex = 0;
		}
	}
