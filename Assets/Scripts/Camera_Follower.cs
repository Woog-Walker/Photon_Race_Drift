using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follower : MonoBehaviour
{
    [SerializeField] float move_smooth;
    [SerializeField] float rot_smooth;
    [Space]
    [SerializeField] Vector3 move_offset; 
    [SerializeField] Vector3 rotation_offset;
    [Space]
    [SerializeField] Transform car_target;

    private void FixedUpdate()
    {
        Follow_Car();
        Rotate_To_Car();
    }

    void Follow_Car()
    {
        Vector3 target_pos = new Vector3();
        target_pos = car_target.TransformPoint(move_offset);

        transform.position = Vector3.Lerp(transform.position, target_pos, move_smooth * Time.deltaTime);
    }

    void Rotate_To_Car()
    {
        var direction = car_target.position - transform.position;
        var rotation = new Quaternion();

        rotation = Quaternion.LookRotation(direction + rotation_offset, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rot_smooth * Time.deltaTime);
    }
}