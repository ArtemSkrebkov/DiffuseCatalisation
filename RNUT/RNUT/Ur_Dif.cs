﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;

namespace RNUT
{
    class Ur_Dif
    {
        List<double[]> u1_list, u2_list;
        double p, k, y, v, c, lymb1, lymb2, t, h;
        double a, b;
        double[] l, coef_u1,coef_u2;
        int count_fun;
        int n;
        int step = 0;
        int u1_x_0;
        int u2_x_0;
        double[] points;
        double cos(double x, int num_fun)
        {
            double res = 0;
            if (num_fun == 0)
            {
                for (int i = 0; i < count_fun; i++)
                {
                    res += coef_u1[i] * Math.Cos(Math.PI * x * l[i]);
                }
            }
            else
            {
                for (int i = 0; i < count_fun; i++)
                {
                    res += coef_u2[i] * Math.Cos(Math.PI * x * l[i]);
                }
            }
            return res;
        }
        double sin(double x)
        {
            return Math.Pow(Math.Sin(Math.PI * x), 2) + 1;
        }
        public Ur_Dif(double[] l_, double[] coef_u1_, double[] coef_u2_, int count_fun_)
        {
            count_fun = count_fun_;
            l = new double[count_fun];
            coef_u1 = new double[count_fun];
            coef_u2 = new double[count_fun];
            for (int i = 0; i < count_fun; i++ )
            {
                l[i] = l_[i];
                coef_u1[i] = coef_u1_[i];
                coef_u2[i] = coef_u2_[i];
            }
            a = 0;
            b = 1;
        }
        public void set_p(double p_)
        {
            p = p_;
        }
        public void set_k(double k_)
        {
            k = k_;
        }
        public void set_y(double y_)
        {
            y = y_;
        }
        public void set_c(double c_)
        {
            c = c_;
        }
        public void set_lymb1(double lymb1_)
        {
            lymb1 = lymb1_;
        }
        public void set_lymb2(double lymb2_)
        {
            lymb2 = lymb2_;
        }
        public void set_t(double t_)
        {
            t = t_;
        }
        public void set_v(double v_)
        {
            v = v_;
        }
        public void set_n(int n_)
        {
            n = n_;
            h = (b - a) / (double)n;
            points = new double[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                points[i] = a + h * i;
            }
            set_u();
        }
        void set_u()
        {
            double[] u1, u2;
            u1 = new double[n + 1];
            u2 = new double[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                u1[i] = cos(a + h * i, 0);
                u2[i] = cos(a + h * i, 1);
            }
            u1_list = new List<double[]>();
            u2_list =new List<double[]>();
            u1_list.Add(u1);
            u2_list.Add(u2);
        }
        public void start(int m)
        {
            double[] u1, u2, u1_old, u2_old;
            u1 = new double[n + 1];
            u2 = new double[n + 1];
            u1_old = new double[n + 1];
            u2_old = new double[n + 1];
            for (int i = 0; i < n + 1; i++)
            {
                u1_old[i] = u1_list[step][i];
                u2_old[i] = u2_list[step][i];
            }
            for (int j = 0; j < m; j++)
            {
                for (int i = 1; i < n; i++)
                {
                    u1[i] = t * (lymb1 * (u1_old[i + 1] - 2 * u1_old[i] + u1_old[i - 1]) / (h * h)
                        + k * u1_old[i] * u1_old[i] / u2_old[i] - y * u1_old[i] + p)
                        + u1_old[i];
                    u2[i] = t * (lymb2 * (u2_old[i + 1] - 2 * u2_old[i] + u2_old[i - 1]) / (h * h)
                        + c * u1_old[i] * u1_old[i] - v * u2_old[i])
                        + u2_old[i];
                }
                u1[0] = u1[1];
                u1[n] = u1[n - 1];
                u2[0] = u2[1];
                u2[n] = u2[n - 1];
                for (int i = 0; i < n + 1; i++)
                {
                    u1_old[i] = u1[i];
                    u2_old[i] = u2[i];
                }
            }
            
            u1_list.Add(u1);
            u2_list.Add(u2);
            step++;
        }
        public void plot(ZedGraphControl zGraph )
        {
            zGraph.GraphPane.CurveList.Clear();
            PointPairList u = new PointPairList();
            PointPairList v = new PointPairList();
            for (int i = 0; i < n + 1; i++)
            {
                u.Add(points[i], u1_list[step][i]);
                v.Add(points[i], u2_list[step][i]);
            }
            zGraph.GraphPane.AddCurve("u2(x)", v, System.Drawing.Color.Red, SymbolType.None);
            zGraph.GraphPane.AddCurve("u1(x)", u, System.Drawing.Color.Green, SymbolType.None);
            zGraph.AxisChange();
            zGraph.Invalidate();
        }
    }
}
