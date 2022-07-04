using System;
using UnityEngine;

namespace CustomMath
{
    public struct Quat : IEquatable<Quat>
    {
        #region Variables

        public float x;
        public float y;
        public float z;
        public float w;

        #endregion

        #region Constants

        public const float kEpsilon = 1E-06F;

        #endregion

        #region Default Values

        public static Quat Identity => new Quat(0, 0, 0, 1);

        #endregion

        #region Constructors

        public Quat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        #endregion

        #region Operators

        public static bool operator ==(Quat lhs, Quat rhs) => IsEqualUsingDot(Dot(lhs, rhs));

        public static bool operator !=(Quat lhs, Quat rhs) => !(lhs == rhs);

        //Multiplicacion de Quat
        public static Quat operator *(Quat lhs, Quat rhs)
        {
            float w = lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z; // Real
            float x = lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y; // imaginario I
            float y = lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z; // imaginario J
            float z = lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x; // imaginario K

            return new Quat(x, y, z, w);
        }

        //Multiplica el Quaternion con un vec3
        //y devuelve una copia del Vec3 con la rotacion del Quat
        public static Vec3 operator *(Quat rotation, Vec3 point)
        {
            float rotX = rotation.x * 2f;
            float rotY = rotation.y * 2f;
            float rotZ = rotation.z * 2f;

            float rotX2 = rotation.x * rotX;
            float rotY2 = rotation.y * rotY;
            float rotZ2 = rotation.z * rotZ;

            float rotXY = rotation.x * rotY;
            float rotXZ = rotation.x * rotZ;
            float rotYZ = rotation.y * rotZ;

            float rotWX = rotation.w * rotX;
            float rotWY = rotation.w * rotY;
            float rotWZ = rotation.w * rotZ;

            Vec3 result = Vec3.Zero;

            result.x = (1f - (rotY2 + rotZ2)) * point.x + (rotXY - rotWZ) * point.y + (rotXZ + rotWY) * point.z;
            result.y = (rotXY + rotWZ) * point.x + (1f - (rotX2 + rotZ2)) * point.y + (rotYZ - rotWX) * point.z;
            result.z = (rotXZ - rotWY) * point.x + (rotYZ + rotWX) * point.y + (1f - (rotX2 + rotY2)) * point.z;

            return result;
        }

        public static implicit operator Quaternion(Quat quat)
        {
            return new Quaternion(quat.x, quat.y, quat.z, quat.w);
        }

        public static implicit operator Quat(Quaternion quaternion)
        {
            return new Quat(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        #endregion

        #region Functions

        //Devuelve los angulos de euler de un Quat
        public Vec3 EulerAngles
        {
            get => ToEulerAngles(this) * Mathf.Rad2Deg;

            set => this = ToQuaternion(value * Mathf.Deg2Rad);
        }

        //Devuelve una copia del Quat ya normalizado.
        public Quat Normalized => Normalize(this);

        public static Quat Euler(float x, float y, float z) => ToQuaternion(new Vec3(x, y, z) * Mathf.Deg2Rad);

        public static Quat Euler(Vec3 euler) => ToQuaternion(euler);

        //De vec3 a Quat
        private static Quat ToQuaternion(Vec3 vec3) // yaw (Z), pitch (Y), roll (X)
        {
            float cz = Mathf.Cos(Mathf.Deg2Rad * vec3.z / 2);   //La parte real del Quat se calcula con el coseno de la mitad del angulo, pasado a radianes 
            float sz = Mathf.Sin(Mathf.Deg2Rad * vec3.z / 2);   //xq asi laburan los Quat

            float cy = Mathf.Cos(Mathf.Deg2Rad * vec3.y / 2);   //La parte imaginaria se calcula con el seno
            float sy = Mathf.Sin(Mathf.Deg2Rad * vec3.y / 2);   

            float cx = Mathf.Cos(Mathf.Deg2Rad * vec3.x / 2);  //Estas son las rotaciones de cada eje
            float sx = Mathf.Sin(Mathf.Deg2Rad * vec3.x / 2);

            Quat quat = new Quat();    

            quat.w = cx * cy * cz + sx * sy * sz;   //Se le agregan las rotaciones de cada eje, multiplicandolas, ya que es la forma de aplicar rotaciones
            quat.x = sx * cy * cz - cx * sy * sz;   
            quat.y = cx * sy * cz + sx * cy * sz;
            quat.z = cx * cy * sz - sx * sy * cz;

            return quat;
        }

        //De quat a vec3
        private static Vec3 ToEulerAngles(Quat quat) //Si no me equivoco, el de unity tiene gimbal lock
        {
            Vec3 angles;

            // roll (x-axis rotation)
            float sinr_cosp = 2 * (quat.w * quat.x + quat.y * quat.z);
            float cosr_cosp = 1 - 2 * (quat.x * quat.x + quat.y * quat.y);
            angles.x = Mathf.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            float sinp = 2 * (quat.w * quat.y - quat.z * quat.x);    //Ojo por el gimbal lock (EXPLICO EN UNITY)

            if (Mathf.Abs(sinp) >= 1)
                angles.y = (Mathf.PI / 2) * Mathf.Sign(sinp); // use 90 degrees if out of range
            else
                angles.y = Mathf.Asin(sinp);

            // yaw / z
            float siny_cosp = 2 * (quat.w * quat.z + quat.x * quat.y);  //Se aplican en desorden las rotaciones, quiso q le queden como en wiki xd
            float cosy_cosp = 1 - 2 * (quat.y * quat.y + quat.z * quat.z);
            angles.z = Mathf.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }


        //Invierte la rotacion del quaternion.
        public static Quat Inverse(Quat rotation)
        {
            Quat q;
            q.w = rotation.w;
            q.x = -rotation.x;
            q.y = -rotation.y;
            q.z = -rotation.z;
            return q;
        }

        //Devuelve un quat normalizado.
        public static Quat Normalize(Quat quat)
        {
            float sqrtDot = Mathf.Sqrt(Dot(quat, quat));

            if (sqrtDot < Mathf.Epsilon)
            {
                return Identity;
            }

            return new Quat(quat.x / sqrtDot, quat.y / sqrtDot, quat.z / sqrtDot, quat.w / sqrtDot);
        }

        //Normaliza el Quat.
        public void Normalize() => this = Normalize(this);

        public static Quat Lerp(Quat a, Quat b, float t) => LerpUnclamped(a, b, Mathf.Clamp01(t));

        public static Quat LerpUnclamped(Quat a, Quat b, float t)
        {
            Quat r;
            float time = 1 - t;
            r.x = time * a.x + t * b.x;
            r.y = time * a.y + t * b.y;
            r.z = time * a.z + t * b.z;
            r.w = time * a.w + t * b.w;

            r.Normalize();

            return r;
        }

        // https://www.youtube.com/watch?v=dttFiVn0rvc&list=PLW3Zl3wyJwWNWsJIPZrmY19urkYHXOH3N

        //Interpola esféricamente entre a y b por t. El parámetro t está sujeto al rango [0, 1].
        public static Quat Slerp(Quat a, Quat b, float t) => SlerpUnclamped(a, b, Mathf.Clamp01(t));

        //Interpola esféricamente entre a y b por t. El parámetro t no está sujeto.

        public static Quat SlerpUnclamped(Quat a, Quat b, float t)
        {
            Quat r;

            float time = 1 - t;

            float wa, wb; //punto de origen y final

            float theta = Mathf.Acos(Dot(a, b)); //Con coseno sacas la parte real del Quat, con arcoseno, a partir de la w sacas el angulo

            if (theta < 0)
            {
                theta = -theta; //Lo invertis, o sea si tenes -30 grados, seria 30 grados, sacas el absoluto
            }

            float sn = Mathf.Sin(theta);

            wa = Mathf.Sin(time * theta) / sn;   
            wb = Mathf.Sin((1 - time) * theta) / sn;

            r.x = wa * a.x + wb * b.x;
            r.y = wa * a.y + wb * b.y;
            r.z = wa * a.z + wb * b.z;
            r.w = wa * a.w + wb * b.w;

            r.Normalize();

            return r;
        }

        //Devuelve el angulo entre 2 Quat en grados.
        public static float Angle(Quat a, Quat b)
        {
            // Se calcula el producto punto para saber si los quaterniones tienen la misma orientacion, si la tienen entonces el angulo es 0.
            float dot = Dot(a, b);

            // Se busca el numero mas chico entre el absoluto del producto punto y 1.
            // Cuando se consigue eso se calcula el arco coseno en radianes.
            // Se realizan las multiplicaciones para conseguir el angulo en grados.

            return IsEqualUsingDot(dot) ? 0f : (Mathf.Acos(Mathf.Min(Mathf.Abs(dot), 1f)) * 2f * Mathf.Rad2Deg);
        }

        private static bool IsEqualUsingDot(float dot) => dot > 0.999999f; // uso este numero constante para darle un margen a la presicion flotante.

        //Devuelve el producto Punto entre 2 quat
        public static float Dot(Quat a, Quat b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

        // From https://stackoverflow.com/questions/12435671/quaternion-lookat-function

        //Forward es el eje z
        //Forma una rotación que parte desde la identidad, hasta forward, teniendo en cuenta, cuál es el vector que se utiliza como ortogonal a donde se está mirando.
        //generalmente, el vector ortogonal que se usa, es el up (0,1,0);

        public static Quat LookRotation(Vec3 sourcePoint, Vec3 destPoint)  //El forward y el up, el up es nuestra direccion que queremos apuntar
        {
            Vec3 dir = Vec3.Normalize(destPoint - sourcePoint);
            Vec3 rotAxis = Vec3.Cross(Vec3.Forward, dir);
            float dot = Vec3.Dot(Vec3.Forward, dir);

            Quat result;
            result.x = rotAxis.x;
            result.y = rotAxis.y;
            result.z = rotAxis.z;

            result.w = dot + 1; //Se hace el +1 ya que, sino, vas a tener el quat con el doble de rotacion (ver imagen)

            return result.Normalized;
        }

        public static Quat LookRotation(Vec3 forward) => LookRotation(forward, Vec3.Up);

        public static Quat RotateTowards(Quat from, Quat to, float maxDegreesDelta)
        {
            float angle = Angle(from, to);

            if (angle == 0f)
            {
                return to;
            }

            return SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / angle));
        }

        #endregion

        #region Internals

        public override bool Equals(object other)
        {
            if (!(other is Quat))
            {
                return false;
            }

            return Equals((Quat)other);
        }

        public bool Equals(Quat other)
        {
            return x.Equals(other.x) &&
                   y.Equals(other.y) &&
                   z.Equals(other.z) &&
                   w.Equals(other.w);
        }

        public override string ToString() => $"({x:0.0},{y:0.0},{z:0.0},{w:0.0})";

        public override int GetHashCode() => x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        #endregion
    }
}