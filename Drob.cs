using System;
using System.Data;
using System.Formats.Asn1;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices;
//using Godot;


//[Serializable]
public class Drob {
    public static Drob Zero = new Drob(0, 1);

    private bool is_sokr = false;
    private BigInteger top = 0;
    private BigInteger down = 1;

    public BigInteger Top {
        get {
            //NormZnak();
            Sokr();
            return top;
        }
        set {
            this.top = value;
        }
    }

    public BigInteger Down {
        get {
            //NormZnak();
            Sokr();
            return down;
        }
        set {
            if (value == 0) {
                throw new DivideByZeroException("down cant be 0");
            }
            this.down = value;
        }
    }

    public Drob() { }

    public Drob(BigInteger top, BigInteger down) {
        if (down == 0) {
            throw new DivideByZeroException("down cant be 0");
        }
        this.top = top;
        this.down = down;

        NormZnak();
    }

    public Drob(string top, string down) {
        if (!BigInteger.TryParse(top, out this.top) || !BigInteger.TryParse(down, out this.down)) {
            throw new ArgumentException("cant convert from string to integer.");
        }
        if (this.down == 0) {
            throw new DivideByZeroException("down cant be 0");
        }

        NormZnak();
    }

    public Drob(Drob d) {
        this.top = d.top;
        this.down = d.down;
    }

    public Drob(string str) {
        if (str == null)
            throw new ArgumentException("str is empty");

        str = str.Trim();
        if (str == "")
            return;


        if (BigInteger.TryParse(str, out this.top)) {
            return;
        }

        String[] splited;
        bool is_neg = false;
        if (str[0] == '-') {
            is_neg = true;
            splited = str.Substring(1).Split(new char[] { '.', ',' }).Select(x => x.Trim()).ToArray();
        }
        else if (str[0] == '+') {
            splited = str.Substring(1).Split(new char[] { '.', ',' }).Select(x => x.Trim()).ToArray();
        }
        else {
            splited = str.Split(new char[] { '.', ',' }).Select(x => x.Trim()).ToArray();
        }
        if (splited.Length != 2) {
            throw new ArgumentException("not vallid string. too many '.' or ',' .");
        }


        if (splited[0] == "") {
            top = 0;
        }
        else {
            if (splited[0][0] == '-' || splited[0][0] == '+' || !BigInteger.TryParse(splited[0], out this.top)) {
                throw new ArgumentException("top arguments is invalid");
            }
        }

        if (splited[1] == "") {
            down = 1;
        }
        else {
            BigInteger tmp_low;
            if (splited[1][0] == '-' || splited[1][0] == '+' || !BigInteger.TryParse(splited[1], out tmp_low)) {
                throw new ArgumentException("down arguments is invalid");
            }

            this.down = BigInteger.Pow(10, splited[1].Length);

            top = top * down + tmp_low;



            Sokr();
        }


        if (is_neg)
            top = -top;
    }

    /*
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
                        down = uint.Parse(downSplit[0]);

                        for (int i = 0; i < mulDown; i++)
                            down *= 10;

                        down += uint.Parse(downSplit[1]);
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
    */

    public void Sokr() {
        if (is_sokr == true)
            return;

        NormZnak();
        // if(top == 0) {down = 1; return;}

        BigInteger del = Gcd(top, down);
        top /= del;
        down /= del;

        is_sokr = true;
    }

    public void NormZnak() {
        if (this.down < 0) {
            this.top = -this.top;
            this.down = -this.down;
        }
    }

    public override bool Equals(object obj) {
        if (obj == null || obj is not Drob)
            return false;

        Drob drob = (Drob)obj;

        return this == drob;
    }

    public override string ToString() {
        this.Sokr();
        return top + "/" + down;
    }

    public static BigInteger Gcd(BigInteger a, BigInteger b) {
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);

        if (a < b) {
            (b, a) = (a, b);
        }

        while (b > 0) {
            a %= b;
            (b, a) = (a, b);
        }

        return a;
    }

    public override int GetHashCode() {
        this.Sokr();
        return HashCode.Combine(top, down);
    }

    public static Drob operator +(Drob l, Drob r) {
        Drob ret = new Drob();

        if (l.down == r.down) {
            ret.top = l.top + r.top;
            ret.down = l.down;
        }
        else {
            ret.top = l.top * r.down + r.top * l.down;
            ret.down = l.down * r.down;
        }

        if (ret.top == 0) { ret.down = 1; }
        ret.is_sokr = false;
        return ret;
    }

    public static Drob operator -(Drob l, Drob r) {
        Drob ret = new Drob();

        if (l.down == r.down) {
            ret.top = l.top - r.top;
            ret.down = l.down;
        }
        else {
            ret.top = l.top * r.down - r.top * l.down;
            ret.down = l.down * r.down;
        }

        if (ret.top == 0) { ret.down = 1; }
        ret.is_sokr = false;
        return ret;
    }

    public static Drob operator *(Drob l, Drob r) {
        Drob ret = new Drob();

        ret.top = l.top * r.top;
        ret.down = l.down * r.down;

        if (ret.top == 0) { ret.down = 1; }
        ret.is_sokr = false;
        return ret;
    }

    public static Drob operator /(Drob l, Drob r) {
        if (r.top == 0)
            throw new DivideByZeroException("right argument was zero.");

        Drob ret = new Drob();

        if (l.top == r.top) {
            ret.top = r.down;
            ret.down = l.down;
        }
        else if (l.down == r.down) {
            ret.top = l.top;
            ret.down = r.top;
        }
        else {
            ret.top = l.top * r.down;
            ret.down = l.down * r.top;
        }

        if (ret.top == 0) { ret.down = 1; }
        ret.is_sokr = false;
        return ret;
    }

    public static bool operator ==(Drob l, Drob r) {
        if (l is null && r is null)
            return true;
        if (l is null || r is null)
            return false;

        l.NormZnak();
        r.NormZnak();

        if (l.top == r.top && l.down == r.down) {
            return true;
        }

        l.Sokr();
        r.Sokr();

        if (l.top == r.top && l.down == r.down) {
            return true;
        }
        else {
            return false;
        }
    }

    public static bool operator !=(Drob l, Drob r) {
        l.NormZnak();
        r.NormZnak();

        if (l.top == r.top && l.down == r.down) {
            return false;
        }

        l.Sokr();
        r.Sokr();

        if (l.top == r.top && l.down == r.down) {
            return false;
        }
        else {
            return true;
        }
    }

    public static implicit operator Drob(int v) {
        return new Drob(v, 1);
    }

    public static implicit operator Drob(long v) {
        return new Drob(v, 1);
    }

    public static implicit operator Drob((int top, int down) d) {
        return new Drob(d.top, d.down);
    }

    public static implicit operator Drob((long top, long down) d) {
        return new Drob(d.top, d.down);
    }

    public static implicit operator (string, string)(Drob d) {
        d.Sokr();
        return (d.top.ToString(), d.down.ToString());
    }

    public static Drob operator -(Drob d) {
        return new Drob(-d.top, d.down); ;
    }

    public static bool operator <(Drob l, Drob r) {
        if (l.Down == r.Down)
            return l.top < r.top;

        Drob tmp_l = new Drob(l);
        Drob tmp_r = new Drob(r);

        tmp_l.top *= r.down;
        tmp_l.down *= r.down;

        tmp_r.top *= l.down;
        tmp_r.down *= l.down;

        tmp_l.NormZnak();
        tmp_r.NormZnak();

        return tmp_l.top < tmp_r.top;
    }

    public static bool operator >(Drob l, Drob r) {
        if (l.Down == r.Down)
            return l.top > r.top;

        Drob tmp_l = new Drob(l);
        Drob tmp_r = new Drob(r);

        tmp_l.top *= r.down;
        tmp_l.down *= r.down;

        tmp_r.top *= l.down;
        tmp_r.down *= l.down;

        tmp_l.NormZnak();
        tmp_r.NormZnak();

        return tmp_l.top > tmp_r.top;
    }
}
