﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SCANsat.Unity.Unity
{
	public class SCAN_DropDown : CanvasFader
	{
		public class OnSelectEvent : UnityEvent<string> { }

		[SerializeField]
		private Transform m_ContentTransform = null;
		[SerializeField]
		private GameObject m_ContentPrefab = null;
		[SerializeField]
		private ScrollRect m_Scrollbar = null;
		[SerializeField]
		private LayoutElement m_Layout = null;

		private string currentElement;
		private OnSelectEvent _onSelectUpdate = new OnSelectEvent();

		protected override void Awake()
		{
			base.Awake();

			Alpha(0);
		}

		public OnSelectEvent OnSelectUpdate
		{
			get { return _onSelectUpdate; }
		}
	
		public void Setup(IList<string> elements, string current)
		{
			if (elements == null)
				return;

			if (m_Layout != null)
			{
				float height = elements.Count * 25;

				height += 5;

				if (height > 155)
					height = 155;

				m_Layout.preferredHeight = height;
			}

			currentElement = current;

			AddElements(elements);

			FadeIn();
		}

		public void FadeIn()
		{
			Fade(1, true);
		}

		public void FadeOut(bool fast = false)
		{
			Fade(0, fast, Close, false);
		}

		private void Close()
		{
			gameObject.SetActive(false);
			DestroyImmediate(gameObject);
		}

		public void SetElement(string element)
		{
			_onSelectUpdate.Invoke(element);
		}

		private void AddElements(IList<string> elements)
		{
			if (m_ContentPrefab == null || m_ContentTransform == null)
				return;

			for (int i = 0; i < elements.Count; i++)
			{
				string element = elements[i];

				AddElement(element);
			}
		}

		private void AddElement(string element)
		{
			SCAN_DropDownElement dropDown = Instantiate(m_ContentPrefab).GetComponent<SCAN_DropDownElement>();

			if (dropDown == null)
				return;

			dropDown.transform.SetParent(m_ContentTransform, false);

			dropDown.Setup(element, element == currentElement, this, m_Scrollbar);
		}
	}
}
