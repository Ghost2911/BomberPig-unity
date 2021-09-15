using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public GameObject explosion;
	public List<Cell> explosiveCells;
	public int damage = 1;

	private void Start()
	{
		StartCoroutine(Explossion());
	}
	IEnumerator Explossion()
	{
		yield return new WaitForSeconds(2f);
		foreach (Cell cell in explosiveCells)
		{
			Instantiate(explosion, cell.transform.position, new Quaternion(0, 0, 0, 0),transform);
			if (cell.go != null)
			{
				if (cell.go.GetComponent<IDamagable>()!=null)
					cell.go.GetComponent<IDamagable>().Damage(damage);
			}

		}
		yield return new WaitForSeconds(0.3f);
		Destroy(gameObject);
	}
}
