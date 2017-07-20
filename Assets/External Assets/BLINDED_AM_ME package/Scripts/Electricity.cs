
//    MIT License
//    
//    Copyright (c) 2017 Dustin Whirle
//    
//    My Youtube stuff: https://www.youtube.com/playlist?list=PL-sp8pM7xzbVls1NovXqwgfBQiwhTA_Ya
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.

using UnityEngine;
using System.Collections;

namespace BLINDED_AM_ME{

	public class Electricity : MonoBehaviour {

		public  float strikeFrequency = 0.5f;
		
		public float smoothness = 0.5f;
		public float zigZagIntensity = 5.0f;
		public float zigZagPerMeter = 5.0f;
		
		public LineRenderer[] lineRenderers;

		private int       _line_iterator = 0;
		private Vector3[] _pathPoints;
		private float     _strikeTracker = 0.0f;


		// Use this for initialization
		void Start () {

			_pathPoints = new Vector3[transform.childCount];
			for(int i=0; i < _pathPoints.Length; i++)
				_pathPoints[i] = transform.GetChild(i).position;

				
		}



		// Update is called once per frame
		void Update () {
		
			_strikeTracker += Time.deltaTime;
			if(_strikeTracker >= strikeFrequency){ // time for another
				_strikeTracker = 0.0f;


				Bolt.Strike(path:_pathPoints,
				lineObject:lineRenderers[_line_iterator],
				zigZagIntensity:zigZagIntensity,
				zigZagPerMeter:zigZagPerMeter,
				smoothness:smoothness);

				lineRenderers[_line_iterator].GetComponent<Animator>().Play("Fade", 0, 0.0f);

				_line_iterator = (_line_iterator + 1) % lineRenderers.Length;
			}
		}

		void OnValidate(){

			smoothness = Mathf.Clamp(smoothness, 0.01f, 1.0f);
			zigZagIntensity = Mathf.Clamp(zigZagIntensity, 0.01f, 100.0f);
			zigZagPerMeter = Mathf.Clamp(zigZagPerMeter, 0.01f, 1000.0f);
		}


		private void OnDrawGizmos()
		{
			DrawGizmos(false);
		}


		private void OnDrawGizmosSelected()
		{
			DrawGizmos(true);
		}


		private void DrawGizmos(bool selected)
		{
			
			if(transform.childCount < 2){

				GameObject point1, point2;
				point1 = new GameObject("point 1");
				point2 = new GameObject("point 2");

				point1.transform.SetParent(transform);
				point2.transform.SetParent(transform);

				point1.transform.localPosition = Vector3.left;
				point2.transform.localPosition = Vector3.right;

			}

			Gizmos.color = selected ? Color.yellow : new Color(1, 1, 0, 0.5f);

			Vector3 prev = transform.GetChild(0).position;

			Transform child;
			for(int i=0; i < transform.childCount; i++){
				child = transform.GetChild(i);
				child.gameObject.name = "point " + i;

				Gizmos.DrawLine(child.position, prev);

				prev = child.position;
			}
				

		}

	}
}