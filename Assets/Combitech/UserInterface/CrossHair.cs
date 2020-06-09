using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class CrossHair : MonoBehaviour
    {
        private Material _mat;

        private void Start() { }

        private void OnPostRender()
        {
            if (Cursor.lockState == CursorLockMode.None)
                return;

            if (!_mat)
            {
                _mat = new Material(Shader.Find("UI/Default"));
            }

            var center = new Vector2(0.5f, 0.5f);
            var dx = 5f / Screen.width;
            var dy = 5f / Screen.height;
            var color = Color.cyan;

            GL.PushMatrix();
            _mat.SetPass(0);
            GL.LoadOrtho();

            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(center + dx * Vector2.left);
            GL.Vertex(center + dx * Vector2.right);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(center + dx * Vector2.left + Vector2.up / Screen.height);
            GL.Vertex(center + dx * Vector2.right + Vector2.up / Screen.height);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(center + dy * Vector2.up);
            GL.Vertex(center + dy * Vector2.down);
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(center + dy * Vector2.up + Vector2.left / Screen.width);
            GL.Vertex(center + dy * Vector2.down + Vector2.left / Screen.width);
            GL.End();


            GL.PopMatrix();
        }
    }
}
