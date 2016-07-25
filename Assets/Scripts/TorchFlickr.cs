/*
 * +================================================================================+
 *      __  ___  ______________  __  __  ______________   ____
 *     / / / / |/ /  _/_  __/\ \/ / / / / /_  __/  _/ /  / __/
 *    / /_/ /    // /  / /    \  / / /_/ / / / _/ // /___\ \
 *    \____/_/|_/___/ /_/     /_/  \____/ /_/ /___/____/___/
 *
 * +================================================================================+
 *
 *    "THE BEER-WARE LICENSE"
 *    -----------------------
 *
 *    As long as you retain this notice you can do whatever you want with this
 *    stuff. If you meet any of the authors some day, and you think this stuff is
 *    worth it, you can buy them a beer in return.
 *
 *    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *    THE SOFTWARE.
 *
 * +================================================================================+
 *
 *    Authors:
 *    David Krummenacher (dave@timbookedtwo.com)
 *
 */


using UnityEngine;
using System.Collections;
//using Utils.Helpers;

/// <summary>
/// Torch whiggler. It's not pretty but it works.
/// </summary>
public class TorchFlickr : MonoBehaviour {
	
	[Header("Light whiggle")]
	/// <summary>
	/// Reference to the light.
	/// </summary>
	public Light TorchLight;
	/// <summary>
	/// The maximum light intensity to be added from the original intensity.
	/// </summary>
	public float MaxIntensityAddition = 0.1f;
	/// <summary>
	/// The maximum light intensity to be subtracted from the original intensity.
	/// </summary>
	public float MaxIntensitySubtraction = 0.1f;
	/// <summary>
	/// The maximum light position to be added from the original position.
	/// </summary>
	public Vector3 MaxPositionAddition = new Vector3(0.005f, 0.005f, 0.005f);
	/// <summary>
	/// The maximum light position to be subtracted from the original position.
	/// </summary>
	public Vector3 MaxPositionSubtraction = new Vector3(0.005f, 0.005f, 0.005f);
	/// <summary>
	/// The minimum speed.
	/// </summary>
	public float MinSpeed = 8f;
	/// <summary>
	/// The maximum speed.
	/// </summary>
	public float MaxSpeed = 12f;
	[Header("Hand whiggle")]
	/// <summary>
	/// The hand position whiggle speed.
	/// </summary>
	public float HandPositionSpeed = 2f;
	/// <summary>
	/// The hand position whiggle amount.
	/// </summary>
	public float HandPositionWhiggleAmount = 0.05f;
	/// <summary>
	/// The hand rotation whiggle speed.
	/// </summary>
	public float HandRotationSpeed = 1f;
	/// <summary>
	/// The hand rotation whiggle amount.
	/// </summary>
	public float HandRotationWhiggleAmount = 5f;
	
	/// <summary>
	/// The minimum intensity amount.
	/// </summary>
	private float minIntensityAmount;
	/// <summary>
	/// The maximum intensity amount.
	/// </summary>
	private float maxIntensityAmount;
	/// <summary>
	/// The target intensity amount.
	/// </summary>
	private float targetIntensityAmount;

	/// <summary>
	/// The minimum position.
	/// </summary>
	//private Vector3 minPosition;
	/// <summary>
	/// The maximum position.
	/// </summary>
	//private Vector3 maxPosition;

	/// <summary>
	/// The target position.
	/// </summary>
	private Vector3 targetPosition;
	
	
	
	/// <summary>
	/// The original rotation.
	/// </summary>
//	private Quaternion originalRotation;
	
	/// <summary>
	/// The speed for the light whiggle.
	/// </summary>
	private float speed;
	/// <summary>
	/// The time for the light whiggle.
	/// </summary>
	private float time;
	
	void Start() {
//		originalRotation = transform.rotation;
		
		// Set inital amounts
		minIntensityAmount = TorchLight.intensity - MaxIntensitySubtraction;
		maxIntensityAmount = TorchLight.intensity + MaxIntensityAddition;
		//minPosition = TorchLight.transform.position - MaxPositionSubtraction;
		//maxPosition = TorchLight.transform.position + MaxPositionAddition;
		
		TorchLight.intensity = Random.Range(minIntensityAmount, maxIntensityAmount);
		Reset();
	}
	
	void Update() {
		// Light whiggle
		time += Time.deltaTime;
		float t = time * speed;
		
		TorchLight.intensity = Mathf.Lerp(TorchLight.intensity, targetIntensityAmount, t);
		
		if (t > 1f) Reset ();
	}
	
	void Reset() {
		// Reset everything for a new light whiggle cycle
		time = 0f;
		targetIntensityAmount = Random.Range(minIntensityAmount, maxIntensityAmount);
		speed = Random.Range(MinSpeed, MaxSpeed);
	}
}