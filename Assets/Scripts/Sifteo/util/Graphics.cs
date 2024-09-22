using System;
using Sifteo;

namespace Aniballs.Sifteo.Util
{
	public class Graphics
	{
		public static readonly Color Red = new Color(255, 0, 0);
		public static readonly Color Green = new Color(0, 255, 0);
		public static readonly Color Blue = new Color(0, 0, 255);
		
		public Graphics ()
		{
		}
		
		public static void Border(Cube cube, Color color, int thickness) {
			cube.FillRect(color, 1, 1, 128 - thickness, thickness);
			cube.FillRect(color, 128 - thickness, 1, thickness, 128 - thickness);
			cube.FillRect(color, 1, thickness, thickness, 128 - thickness);
			cube.FillRect(color, thickness, 128 - thickness, 128 - thickness, thickness);
		}
		
		public static void Rect(Cube cube, Color color, int x, int y, int width, int height) {
			cube.FillRect(color, x,             y,              width, 1);
			cube.FillRect(color, x + width - 1, y,              1,     height);
			cube.FillRect(color, x + 1,         y + height - 1, width - 1, 1);
			cube.FillRect(color, x,             y + 1,          1,     height - 1);
		}
		/*
		 *  !"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~
		 */
	}
}

