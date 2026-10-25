using System;
using System.Data;
using System.Runtime.InteropServices;
using Godot;

namespace MainNameSpace
{
    [Serializable]
    public class Drob
    {
        int top = 0;
        int down = 1;

        public Drob() { }

        public Drob(int top, int down)
        {
            if (down == 0)
            {
                throw new DivideByZeroException("down cant be 0");
            }
            this.top = top;
            this.down = down;
        }

        public Drob(string str)
        {
            if (str == null)
                throw new NoNullAllowedException("str was null");

            string[] parsed = str.Split("/");
            if (parsed.Length > 2)
                throw new ArgumentException("format shoot be top/down");

            int mulTop = 0;
            if (!int.TryParse(parsed[0], out top))
            {
                string[] topSplit = parsed[0].Split('.');
                if (topSplit.Length > 2)
                {
                    throw new ArgumentException("top format shoot be int.int or int");
                }

                mulTop = topSplit[1].Length;
                top = int.Parse(topSplit[0]);

                for (int i = 0; i < mulTop; i++)
                    top *= 10;

                top += int.Parse(topSplit[1]);
            }

            if (parsed.Length > 1)
            {
                int mulDown = 0;
                if (!int.TryParse(parsed[1], out top))
                {
                    string[] downSplit = parsed[0].Split('.');
                    if (downSplit.Length > 2)
                    {
                        throw new ArgumentException("down format shoot be int.int or int");
                    }

                    mulDown = downSplit[1].Length;
                    down = int.Parse(downSplit[0]);

                    for (int i = 0; i < mulDown; i++)
                        down *= 10;

                    down += int.Parse(downSplit[1]);
                }

                if (mulTop > mulDown)
                {
                    for (int i = 0; i < (mulTop - mulDown); i++)
                        down *= 10;
                }
                else if (mulDown > mulTop)
                {
                    for (int i = 0; i < (mulDown - mulTop); i++)
                        top *= 10;
                }
            }
        }

        public static Drob operator +(Drob l, Drob r)
        {
            int top, down;
            try
            {
                checked
                {
                    if (l.down == r.down)
                    {
                        top = l.top + r.top;
                        down = l.down;
                    }
                    else
                    {
                        top = l.top * r.down + r.top * l.down;
                        down = l.down * r.down;
                    }
                }
            }
            catch (OverflowException)
            {
                l.Sokr();
                r.Sokr();

                checked
                {
                    if (l.down == r.down)
                    {
                        top = l.top + r.top;
                        down = l.down;
                    }
                    else
                    {
                        top = l.top * r.down + r.top * l.down;
                        down = l.down * r.down;
                    }
                }
            }

            return new Drob(top, down);
        }

        public static Drob operator -(Drob l, Drob r)
        {
            int top, down;
            try
            {
                checked
                {
                    if (l.down == r.down)
                    {
                        top = l.top - r.top;
                        down = l.down;
                    }
                    else
                    {
                        top = l.top * r.down - r.top * l.down;
                        down = l.down * r.down;
                    }
                }
            }
            catch (OverflowException)
            {
                l.Sokr();
                r.Sokr();

                checked
                {
                    if (l.down == r.down)
                    {
                        top = l.top - r.top;
                        down = l.down;
                    }
                    else
                    {
                        top = l.top * r.down - r.top * l.down;
                        down = l.down * r.down;
                    }
                }
            }

            return new Drob(top, down);
        }

        public static Drob operator *(Drob l, Drob r)
        {
            int top, down;
            try
            {
                checked
                {
                    top = l.top * r.top;
                    down = l.down * r.down;
                }
            }
            catch (OverflowException)
            {
                l.Sokr();
                r.Sokr();

                checked
                {
                    top = l.top * r.top;
                    down = l.down * r.down;
                }
            }

            return new Drob(top, down);
        }

        public static Drob operator /(Drob l, Drob r)
        {
            int top, down;
            try
            {
                checked
                {
                    top = l.top * r.down;
                    down = l.down * r.top;
                }
            }
            catch (OverflowException)
            {
                l.Sokr();
                r.Sokr();

                checked
                {
                    top = l.top * r.down;
                    down = l.down * r.top;
                }
            }

            return new Drob(top, down);
        }

        public static bool operator ==(Drob l, Drob r)
        {
            if (l.top == r.top && l.down == r.down)
                return true;

            l.Sokr();
            r.Sokr();

            if (l.top == r.top && l.down == r.down)
                return true;

            return false;
        }

        public static bool operator !=(Drob l, Drob r)
        {
            if (l.top != r.top || l.down != r.down)
                return true;

            l.Sokr();
            r.Sokr();

            if (l.top != r.top || l.down != r.down)
                return true;

            return false;
        }

        public static Drob operator ++(Drob d)
        {
            try
            {
                checked
                {
                    return new Drob(d.top + 1, d.down);
                }
            }
            catch (OverflowException)
            {
                d.Sokr();
                checked
                {
                    return new Drob(d.top + 1, d.down);
                }
            }
        }

        public static Drob operator --(Drob d)
        {
            try
            {
                checked
                {
                    return new Drob(d.top + 1, d.down);
                }
            }
            catch (OverflowException)
            {
                d.Sokr();
                checked
                {
                    return new Drob(d.top + 1, d.down);
                }
            }
        }

        public override string ToString()
        {
            this.Sokr();
            return top + "/" + down;
        }

        public void Sokr()
        {
            int del = Gcd(top, down);
            top /= del;
            down /= del;
        }

        public static int Gcd(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }
    }


}