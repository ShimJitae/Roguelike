using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    //읽기는 누구나 가능하고 자식만 수정가능
    public float maxHp { get; protected set; }
    public float hp {  get; protected set; }
    public float attack { get; protected set; }
    public float defense { get; protected set; }

}
