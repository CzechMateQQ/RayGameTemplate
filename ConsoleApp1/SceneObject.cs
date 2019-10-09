using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Raylib;
using rl = Raylib.Raylib;

namespace MatrixHeirarchy
{
    public class SceneObject
    {
        protected SceneObject parent = null;
        protected List<SceneObject> children = new List<SceneObject>();

        protected Matrix3 localTransform = new Matrix3();
        protected Matrix3 globalTransform = new Matrix3();

        public Matrix3 LocalTransform
        {
            get { return localTransform; }
        }

        public Matrix3 GlobalTransform
        {
            get { return globalTransform; }
        }

        void UpdateTransform()
        {
            if(parent != null)
            {
                globalTransform = parent.globalTransform * localTransform;
            }
            else
            {
                globalTransform = localTransform;
            }

            foreach(SceneObject child in children)
            {
                child.UpdateTransform();
            }
        }

        public void SetPosition(float x, float y)
        {
            localTransform.SetTranslation(x, y);
            UpdateTransform();
        }

        public void SetRotate(float radians)
        {
            localTransform.SetRotateZ(radians);
            UpdateTransform();
        }
        public void SetScale(float width, float height)
        {
            localTransform.SetScaled(width, height, 1);
            UpdateTransform();
        }
        public void Translate(float x, float y)
        {
            localTransform.Translate(x, y);
            UpdateTransform();
        }
        public void Rotate(float radians)
        {
            localTransform.RotateZ(radians);
            UpdateTransform();
        }
        public void Scale(float width, float height)
        {
            localTransform.Scale(width, height, 1);
            UpdateTransform();
        }

        public SceneObject Parent
        {
            get { return parent; }
        }

        public SceneObject()
        {
            if(parent != null)
            {
                parent.RemoveChild(this);
            }

            foreach(SceneObject so in children)
            {
                so.parent = null;
            }
        }

        public int GetChildCount()
        {
            return children.Count;
        }

        public SceneObject GetChild(int index)
        {
            return children[index];
        }

        public void AddChild(SceneObject child)
        {
            //Make sure child doesn't already have a parent
            Debug.Assert(child.parent == null);

            child.parent = this;

            children.Add(child);
        }

        public void RemoveChild(SceneObject child)
        {
            if(children.Remove(child) == true)
            {
                child.parent = null;
            }
        }

        public virtual void OnUpdate(float deltaTime)
        {

        }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);

            foreach(SceneObject child in children)
            {
                child.Update(deltaTime);
            }
        }

        public virtual void OnDraw()
        {

        }

        public void Draw()
        {
            OnDraw();

            foreach(SceneObject child in children)
            {
                child.Draw();
            }
        }
    }

    public struct Matrix3
    {
        public float x1, y1, z1, 
                     x2, y2, z2, 
                     x3, y3, z3;
        public Matrix3(float a1, float a2, float a3, float b1, float b2, float b3, float c1, float c2, float c3)
        {
            x1 = a1; y1 = b1; z1 = c1;
            x2 = a2; y2 = b2; z2 = c2;
            x3 = a3; y3 = b3; z3 = c3;
        }

        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
        {
            return new Matrix3(
                m1.x1 * m2.x1 + m1.y1 * m2.x2 + m1.z1 * m2.x3,
                m1.x1 * m2.y1 + m1.y1 * m2.y2 + m1.z1 * m2.y3,
                m1.x1 * m2.z1 + m1.y1 * m2.z2 + m1.z1 * m2.z3,
                m1.x2 * m2.x1 + m1.y2 * m2.x2 + m1.z2 * m2.x3,
                m1.x2 * m2.y1 + m1.y2 * m2.y2 + m1.z2 * m2.y3,
                m1.x2 * m2.z1 + m1.y2 * m2.z2 + m1.z2 * m2.z3,
                m1.x3 * m2.x1 + m1.y3 * m2.x2 + m1.z3 * m2.x3,
                m1.x3 * m2.y1 + m1.y3 * m2.y2 + m1.z3 * m2.y3,
                m1.x3 * m2.z1 + m1.y3 * m2.z2 + m1.z3 * m2.z3);
        }

        public void Set(Matrix3 input)
        {
            x1 = input.x1;
            x2 = input.x2;
            x3 = input.x3;
            y1 = input.y1;
            y2 = input.y2;
            y3 = input.y3;
            z1 = input.z1;
            z2 = input.z2;
            z3 = input.z3;
        }

        public void SetScaled(float x, float y, float z)
        {
            x1 = x; y1 = 0; z1 = 0;
            x2 = 0; y2 = y; z2 = 0;
            x3 = 0; y3 = 0; z3 = z;
        }

        public void Scale(float x, float y, float z)
        {
            Matrix3 m = new Matrix3();
            m.SetScaled(x, y, z);
        }

        public void SetTranslation(float x, float y)
        {
            x3 = x; y3= y; z3 = 1;
        }

        public void Translate(float x, float y)
        {
            x3 += x; y3 += y;
        }

        public void SetRotateZ(double radians)
        {
            Set(new Matrix3((float)Math.Cos(radians), (float)Math.Sin(radians), 0,
                (float)-Math.Sin(radians), (float)Math.Cos(radians), 0,
                0, 0, 1));
        }

        public void RotateZ(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateZ(radians);

            Set(this * m);
        }
    }
}
