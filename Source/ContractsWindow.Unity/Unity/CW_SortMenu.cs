﻿#region license
/*The MIT License (MIT)
CW_SortMenu - Controls the sort menu popup

Copyright (c) 2016 DMagic

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using System.Collections.Generic;
using ContractsWindow.Unity.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace ContractsWindow.Unity.Unity
{
	[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
	public class CW_SortMenu : CW_Popup
	{
		private IMissionSection missionInterface;

		public void setSort(IMissionSection m)
		{
			if (m == null)
				return;

			missionInterface = m;

			FadeIn();
		}

		public void SortDifficulty()
		{
			if (CW_Window.Window == null)
				return;

			if (missionInterface == null)
				return;

			missionInterface.SetSort(1);

			CW_Window.Window.FadePopup(this);
		}

		public void SortExpiration()
		{
			if (CW_Window.Window == null)
				return;

			if (missionInterface == null)
				return;

			missionInterface.SetSort(2);

			CW_Window.Window.FadePopup(this);
		}

		public void SortAccept()
		{
			if (CW_Window.Window == null)
				return;

			if (missionInterface == null)
				return;

			missionInterface.SetSort(3);

			CW_Window.Window.FadePopup(this);
		}

		public void SortReward()
		{
			if (CW_Window.Window == null)
				return;

			if (missionInterface == null)
				return;

			missionInterface.SetSort(4);

			CW_Window.Window.FadePopup(this);
		}

		public void SortType()
		{
			if (CW_Window.Window == null)
				return;

			if (missionInterface == null)
				return;

			missionInterface.SetSort(5);

			CW_Window.Window.FadePopup(this);
		}

		public void SortPlanet()
		{
			if (CW_Window.Window == null)
				return;

			if (missionInterface == null)
				return;

			missionInterface.SetSort(6);

			CW_Window.Window.FadePopup(this);
		}

		public override void ClosePopup()
		{
			gameObject.SetActive(false);

			Destroy(gameObject);
		}
	}
}
