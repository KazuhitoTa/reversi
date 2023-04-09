using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car
{
    public class CarCamera : MonoBehaviour
    {
    	[SerializeField] public static Transform target; // 追従する対象のTransform
        [SerializeField] private Vector3 offset; // 追従時のカメラの相対位置
        [SerializeField] private float followSpeed = 10f; // 追従時のカメラの移動速度
        [SerializeField] private float rotationSpeed = 5f; // カメラの回転速度

        private float pitch = 0f; // カメラの仰角

        private void LateUpdate()
        {
            // 追従位置を計算
            Vector3 desiredPosition = target.position + offset;

            // カメラを滑らかに移動
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // カメラの位置を変更
            transform.position = smoothedPosition;

            // 追従対象に向かってカメラを回転
            float yaw = target.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

    }

}
