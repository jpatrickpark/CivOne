// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System.Drawing;
using CivOne.Enums;
using CivOne.Events;
using CivOne.GFX;
using CivOne.Interfaces;
using CivOne.Templates;

namespace CivOne.Screens
{
	internal class CityManager : BaseScreen
	{
		private readonly City _city;

		private readonly Bitmap _background;

		private readonly CityHeader _cityHeader;
		private readonly CityResources _cityResources;
		private readonly CityUnits _cityUnits;
		private readonly CityMap _cityMap;
		private readonly CityBuildings _cityBuildings;
		private readonly CityFoodStorage _cityFoodStorage;
		
		private bool _update = true;
		private bool _redraw = false;

		private void CloseScreen()
		{
			_cityHeader.Close();
			Destroy();
		}
		
		private void DrawLayer(IScreen layer, uint gameTick, int x, int y)
		{
			if (layer == null) return;
			if (!layer.HasUpdate(gameTick) && !_redraw) return;
			AddLayer(layer, x, y);
		}
		
		public override bool HasUpdate(uint gameTick)
		{
			if (_cityHeader.HasUpdate(gameTick)) _update = true;
			if (_cityResources.HasUpdate(gameTick)) _update = true;
			if (_cityUnits.HasUpdate(gameTick)) _update = true;
			if (_cityMap.HasUpdate(gameTick)) _update = true;
			if (_cityBuildings.HasUpdate(gameTick)) _update = true;
			if (_cityFoodStorage.HasUpdate(gameTick)) _update = true;
			
			DrawLayer(_cityHeader, gameTick, 2, 1);
			DrawLayer(_cityResources, gameTick, 2, 23);
			DrawLayer(_cityUnits, gameTick, 2, 67);
			DrawLayer(_cityMap, gameTick, 127, 23);
			DrawLayer(_cityBuildings, gameTick, 211, 1);
			DrawLayer(_cityFoodStorage, gameTick, 2, 106);
			
			if (_update)
			{
				_update = false;
				return true;
			}
			return false;
		}
		
		public override bool KeyDown(KeyboardEventArgs args)
		{
			CloseScreen();
			return true;
		}
		
		public override bool MouseDown(ScreenEventArgs args)
		{
			CloseScreen();
			return true;
		}

		public CityManager(City city)
		{
			_city = city;
			_background = (Bitmap)Resources.Instance.GetPart("SP299", 288, 120, 32, 16).Clone();
			Picture.ReplaceColours(_background, new byte[] { 7, 22 }, new byte[] { 9, 57 });

			Cursor = MouseCursor.Pointer;
			
			Color[] palette = Resources.Instance.LoadPIC("SP257").Image.Palette.Entries;
			
			_canvas = new Picture(320, 200, palette);
			_canvas.FillRectangle(5, 0, 0, 320, 200);
			
			_cityHeader = new CityHeader(_city, _background);
			_cityResources = new CityResources(_city, _background);
			_cityUnits = new CityUnits(_city, _background);
			_cityMap = new CityMap(_city, _background);
			_cityBuildings = new CityBuildings(_city, _background);
			_cityFoodStorage = new CityFoodStorage(_city, _background);
		}
	}
}