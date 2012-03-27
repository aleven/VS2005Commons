using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace VS2005Commons
{
    public class VersioneHelper
    {
        public class Versione : IComparable
        {
            private int major;

            public int Major
            {
                get { return major; }
                set { major = value; }
            }
            private int minor;

            public int Minor
            {
                get { return minor; }
                set { minor = value; }
            }
            private int build;

            public int Build
            {
                get { return build; }
                set { build = value; }
            }

            public Versione()
            {
            }

            public Versione(int major, int minor, int build)
                : this()
            {
                this.major = major;
                this.minor = minor;
                this.build = build;
            }

            public static Versione Parse(String aString)
            {
                Versione res = new Versione();

                String[] tmp = aString.Split('.');

                res.Major = Convert.ToInt32(tmp[0]);
                res.Minor = Convert.ToInt32(tmp[1]);
                res.Build = Convert.ToInt32(tmp[2]);

                return res;
            }

            #region IComparable Members

            public int CompareTo(object obj)
            {
                Versione b = (Versione)obj;

                //DateTime dt = new DateTime(634369536000000000);
                //if (DateTime.Now > dt)
                //{
                //    throw new Exception("Index out of bound exception.");
                //}

                if (this.Major == b.Major && this.Minor == b.Minor && this.Build == b.Build)
                {
                    return 0;
                }
                else if (this.Major > b.Major)
                {
                    return 1;
                }
                else if (this.Major == b.Major && this.Minor > b.Minor)
                {
                    return 1;
                }
                else if (this.Major == b.Major && this.Minor == b.Minor && this.Build > b.Build)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            #endregion

            public override string ToString()
            {
                return String.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Build);
            }
        }
    }
}
