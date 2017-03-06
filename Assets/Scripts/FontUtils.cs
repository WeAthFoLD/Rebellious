using UnityEngine;
using System.Collections.Generic;

public class NumbersDrawer {

    public enum Align { LEFT, CENTER, RIGHT };

    private List<SingleNumber> drawList = new List<SingleNumber>();

    private static Material[] materials;

    private Align align;

    private float sum;

    public NumbersDrawer(Align align = Align.LEFT) {
        if(materials == null) {
            materials = new Material[10];
            for(int i = 0; i < 10; ++i) {
                materials[i] = Resources.Load("Materials/" + i, typeof(Material)) as Material;
            }
        }
        this.align = align;
    }

    public void setNumber(int n) {
        List<int> numbers = new List<int>();

        if(n == 0) {
            numbers.Add(0);
        } else {
            while(n != 0) {
                numbers.Add(n % 10);
                n /= 10;
            }
        }

        drawList.Clear();
        sum = 0.0f;
        for(int i = numbers.Count - 1; i >= 0; --i) {
            int num = numbers[i];
            SingleNumber sn = new SingleNumber(materials[num]);
            drawList.Add(sn);
            sum += materials[num].mainTexture.width;
        }

        if(align == Align.CENTER) {
            sum /= 2;
        } else if(align == Align.LEFT) {
            sum = 0;
        }
    }

    public void draw(float x, float y, float scale) {
        float x0 = x - sum * scale, y0 = y;

        foreach(SingleNumber sn in drawList) {
            sn.draw(x0, y0, scale);
            x0 += sn.mat.mainTexture.width * scale * 0.8f;
        }
    }

    private class SingleNumber {
        public Material mat;

        public SingleNumber(Material mat) {
            this.mat = mat;
        }

        public void draw(float x, float y, float scale) {
            Matrix4x4 mat4 = Matrix4x4.TRS(new Vector3(x, y), Quaternion.identity, new Vector3(scale, scale, 1));

            //scale = 0.01f;
            //x = -25;
            //y = -0;
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[] {
                new Vector3(x, y),
                new Vector3(x + mat.mainTexture.width * scale, y),
                new Vector3(x + mat.mainTexture.width * scale, y + mat.mainTexture.height * scale),
                new Vector3(x, y + mat.mainTexture.height * scale)
            };
            mesh.uv = new Vector2[] {
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)
            };
            mesh.triangles = new int[] { 2, 1, 0, 3, 2, 0 };

            Graphics.DrawMesh(mesh, Matrix4x4.identity, mat, 13);
        }
    }

}
