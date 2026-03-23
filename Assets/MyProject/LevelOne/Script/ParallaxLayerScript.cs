using UnityEngine;

public class ParallaxLayerScript : MonoBehaviour
{
   Material mat;
   float distance;
   [Range(0f, 0.5f)]
   public float speed = 0.2f;

   void Start()
   {
      SpriteRenderer sr = GetComponent<SpriteRenderer>();
      if (sr == null)
      {
         sr = GetComponentInChildren<SpriteRenderer>();
      }
      if (sr == null)
      {
         Debug.LogError($"{nameof(ParallaxLayerScript)} requires a SpriteRenderer on the same GameObject or a child.", this);
         enabled = false;
         return;
      }

      mat = sr.material;
      if (mat == null)
      {
         Debug.LogError($"{nameof(ParallaxLayerScript)}: SpriteRenderer material is null.", this);
         enabled = false;
      }
   }

   void Update()
   {
      if (mat == null) return;
      distance += Time.deltaTime * speed;
      mat.SetTextureOffset("_MainTex", new Vector2(distance, 0));
   }

}
