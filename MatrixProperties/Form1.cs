using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatrixProperties
{

    public partial class Form1 : Form
    {
        private int M,N,sigma_n=0;//количество строк и столбцов
        private float[,] A1;
        private float[,] U1;
        private float[,] V1;
        private float[,] Sigma1;
        private float[] A;
        private float[] U;
        private float[] V;
        private float[] Sigma;
        private float[,] Uh;
        private float[,] Vh;
        private float[,] Sigma_;
        private float[,] A_test;
        private float[,] A_;

        static public int padding = 10;
        static public int left_keys_padding = 20;
        static public int actual_left = 30;
        static public int actual_top = 10;
        private PointF[] sigma_points;
        public Graphics graphic;
        Pen pen1 = new Pen(Color.DarkRed, 2f);
        Pen pen2 = new Pen(Color.Black, 2f);
        private void Form1_Load(object sender, EventArgs e)
        {
            for_M.Text = "3";
            for_N.Text = "2";
            consondeb();

        }
        public Form1()
        {
            InitializeComponent();

        }
        public void LSistema()
        {
            M = Convert.ToInt32(for_M.Text);
            N = Convert.ToInt32(for_N.Text);

        }
        private void button1_Click(object sender, EventArgs e)//построение матрицы
        {

            LSistema();
            A1 = new float[M,N];
                Random rnd = new Random();
                for(int i=0;i<M;i++)
                    for (int j = 0; j < N; j++)
                        A1[i,j] = (float)rnd.NextDouble() ;

            Console.WriteLine("A матрица:");
            for (int row = 0; row < A1.GetLength(0); row++)
            {
                for (int column = 0; column < A1.GetLength(1); column++)
                {
                    string s = float.Parse(A1[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                    //Console.Write(A1[row, column] + "\t");
                }
                Console.WriteLine();
            }
            A = new float[M*N];
            int z = 0;
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                {
                    A[z] = A1[i, j];
                    z++;
                }

        }

        private void button2_Click(object sender, EventArgs e)//разложение матрицы
        {
            if (M < N) sigma_n = M;
            else sigma_n = N;
            U = new float[N * N];
            V = new float[M*M];
            Sigma = new float[sigma_n];
            U1 = new float[N ,N];
            V1 = new float[M, M];
            Sigma1 = new float[M , N];
            int iter=SVD(M,N, A,U,V,Sigma);
            int z = 0;
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    U1[i, j] = U[z];
                    z++;
                }
            z = 0;
            for (int i = 0; i < M; i++)
                for (int j = 0; j < M; j++)
                {
                    V1[i, j] = V[z];
                    z++;
                }
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                {
                    if(i==j) Sigma1[i, j] = Sigma[i]; 
                    else Sigma1[i, j] = 0;
                }
            sigma_points = new PointF[sigma_n];
            for (int i = 0; i < sigma_n; i++)
            {
                sigma_points[i] = new PointF(i ,Sigma1[i,i]);
            }
            graphic = pictureBox1.CreateGraphics();
            PainNet(graphic, pictureBox1, pen2, sigma_points, sigma_n, sigma_n);
            PainGraph(graphic, pictureBox1, pen1, sigma_points, sigma_n, sigma_n); 

            Console.WriteLine("U матрица:");
            for (int row = 0; row < U1.GetLength(0); row++)
            {
                for (int column = 0; column < U1.GetLength(1); column++)
                {
                    string s = float.Parse(U1[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Sigma матрица:");
            for (int row = 0; row < Sigma1.GetLength(0); row++)
            {
                for (int column = 0; column < Sigma1.GetLength(1); column++)
                {
                    string s = float.Parse(Sigma1[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("V матрица:");
            for (int row = 0; row < V1.GetLength(0); row++)
            {
                for (int column = 0; column < V1.GetLength(1); column++)
                {
                    string s = float.Parse(V1[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                }
                Console.WriteLine();
            }
        }
        
        private void button3_Click(object sender, EventArgs e)//свойства обратной матрицы
        {
            A_test = new float[M, N];
            A_= new float[N, M];
            Uh = new float[M , M];
            Vh = new float[N , N];
            Sigma_ = new float[M , N];
            Uh = swap(U1);
            Vh = swap(V1);
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                {
                    if (i == j) Sigma_[i, j] = 1/Sigma[i];
                    else Sigma_[i, j] = 0;
                }
            Console.WriteLine("Uh матрица:");
            for (int row = 0; row < Uh.GetLength(0); row++)
            {
                for (int column = 0; column < Uh.GetLength(1); column++)
                {
                    string s = float.Parse(Uh[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("матрица обратных Sigma:");
            for (int row = 0; row < Sigma_.GetLength(0); row++)
            {
                for (int column = 0; column < Sigma_.GetLength(1); column++)
                {
                    string s = float.Parse(Sigma_[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Vh матрица:");
            for (int row = 0; row < Vh.GetLength(0); row++)
            {
                for (int column = 0; column < Vh.GetLength(1); column++)
                {
                    string s = float.Parse(Vh[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                }
                Console.WriteLine();
            }
            A_ = umn(umn(Uh, Sigma1), V1);
            Console.WriteLine("Псевдообратная? матрица:");
            for (int row = 0; row < A_.GetLength(0); row++)
            {
                for (int column = 0; column < A_.GetLength(1); column++)
                {
                    string s = float.Parse(A_[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                }
                Console.WriteLine();
            }
            A_test = umn(umn(U1, Sigma1), Vh);
            Console.WriteLine("A матрица:");
            for (int row = 0; row < A_test.GetLength(0); row++)
            {
                for (int column = 0; column < A_test.GetLength(1); column++)
                {
                    string s = float.Parse(A_test[row, column].ToString()).ToString("F5");//без округления
                    Console.Write(s.Remove(s.Length - 2) + "\t");
                }
                Console.WriteLine();
            }
            //A_test = AA_(A1, A_);
            //Console.WriteLine("E матрица:");
            //for (int row = 0; row < A_test.GetLength(0); row++)
            //{
            //    for (int column = 0; column < A_test.GetLength(1); column++)
            //    {
            //        string s = float.Parse(A_test[row, column].ToString()).ToString("F5");//без округления
            //        Console.Write(s.Remove(s.Length - 2) + "\t");
            //    }
            //    Console.WriteLine();
            //}
        }


        private void button7_Click(object sender, EventArgs e)//свойства псевдообратной матрицы
        {

        }
        private void button4_Click(object sender, EventArgs e)//ок
        {

        }
        int SVD(int m_m, int n_n, float[]a, float[]u, float[]v, float[]sigma)
        {
            float thr = 0.000001f;
            int n, m, i, j, l, k,  iter, inn, ll, kk;
            bool lort;
            float alfa, betta, hamma, eta, t, cos0, sin0, buf, s, r;
            n = n_n;
            m = m_m;
            for (i = 0; i < n; i++)
            { inn= i * n;
                for (j = 0; j < n; j++)
                    if (i == j) v[inn +j] = 1;
                    else v[inn +j] = 0;
            }
            for (i = 0; i < m; i++)
            {       inn= i * n;
                for (j = 0; j < n; j++)
                    u[inn +j] = a[inn +j];
            }

            iter = 0;
            while (true)
            {
                lort = false;
                iter++;
                for (l = 0; l < n - 1; l++)
                    for (k = l + 1; k < n; k++)
                    {
                        alfa = 0; betta = 0; hamma = 0;
                        for (i = 0; i < m; i++)
                        {
                            inn= i * n;
                            ll =inn+l;
                            kk =inn+k;
                            alfa += u[ll] * u[ll];
                            betta += u[kk] * u[kk];
                            hamma += u[ll] * u[kk];
                        }

                        if (Math.Sqrt(alfa * betta) < (1e-10)) continue;
                        if (Math.Abs(hamma) / Math.Sqrt(alfa * betta) < thr)
                            continue;

                        lort = true;
                        eta = (betta - alfa) / (float)(2.0 * hamma);
                        t = (eta / Math.Abs(eta)) /
                               (Math.Abs(eta) + (float)Math.Sqrt(1.0 + eta * eta));
                        cos0 = (float)1.0 / (float)Math.Sqrt(1.0 + t * t);
                        sin0 = t * cos0;

                        for (i = 0; i < m; i++)
                        {
                                        inn= i * n;
                            buf = u[inn +l] * cos0 - u[inn +k] * sin0;
                            u[inn +k] = u[inn +l] * sin0 + u[inn +k] * cos0;
                            u[inn +l] = buf;

                            if (i >= n) continue;
                            buf = v[inn +l] * cos0 - v[inn +k] * sin0;
                            v[inn +k] = v[inn +l] * sin0 + v[inn +k] * cos0;
                            v[inn +l] = buf;
                        }
                    }

                if (!lort) break;
            }

            for (i = 0; i < n; i++)
            {
                s = 0;
                for (j = 0; j < m; j++) s += u[j * n + i] * u[j * n + i];
                s = (float)(Math.Sqrt(s));
                sigma[i] = s;
                if (s < (1e-10)) continue;
                for (j = 0; j < m; j++) u[j * n + i] = u[j * n + i] / s;
            }
            for (i = 0; i < n - 1; i++)
                for (j = i; j < n; j++)
                    if (sigma[i] < sigma[j])
                    {
                        r = sigma[i]; sigma[i] = sigma[j]; sigma[j] = r;
                        for (k = 0; k < m; k++)
                        { r = u[i + k * n]; u[i + k * n] = u[j + k * n]; u[j + k * n] = r; }
                        for (k = 0; k < n; k++)
                        { r = v[i + k * n]; v[i + k * n] = v[j + k * n]; v[j + k * n] = r; }
                    }

            return iter;
        }
        // Умножениепрямоугольных матриц А на матрицу Б
        public  float[,] umn(float [,] a, float [,] b)
        {
            float[,] resMass = new float[a.GetLength(0), b.GetLength(1)];//32
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    resMass[i, j] = 0;
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        resMass[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return resMass;

        }
        //транспонирование
        public float[,] swap(float[,] a)
        {

            float[,] temp = new float[a.GetLength(1), a.GetLength(0)];

            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = a[j, i];
                }
            }
            return temp;
        }
        //свойства?
        public float[,] AA_(float[,] a, float[,] b)//AA_=E
        {

            float[,] resMass = new float[M, M];
            resMass = umn(a, b);
            return resMass;
        }


        //для консоли
        public void consondeb()
        {
            if (NativeMethods.AllocConsole())
            {
                IntPtr stdHandle = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE);
                //Console.ForegroundColor = ConsoleColor.Black;
                //Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("Привет !\r\n");
            }
            else
            {
                Console.WriteLine("Консоль Активна!");
            }

        }
        public partial class NativeMethods
        {
            public static Int32 STD_OUTPUT_HANDLE = -11;
            [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "GetStdHandle")]
            public static extern System.IntPtr GetStdHandle(Int32 nStdHandle);
            [System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "AllocConsole")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool AllocConsole();
        }
        //РИСОВАЛКА!!!
        public void PainNet(Graphics gr, PictureBox pictureBox, Pen penG, PointF[] points, double toX, int n)//Отрисовка сетки с подписями
        {
            PointF[] copy_points = new PointF[n];
            copy_points = (PointF[])points.Clone();


            int wX, hX;
            wX = pictureBox.Width;
            hX = pictureBox.Height;
            Point KX1 = new Point(30, hX - 10);
            Point KX2 = new Point(wX - 10, hX - 10);
            gr.DrawLine(penG, KX1, KX2);
            Point KY1 = new Point(30, 10);
            Point KY2 = new Point(30, hX - 10);
            gr.DrawLine(penG, KY1, KY2);
            int actual_width = wX - 2 * padding - left_keys_padding;
            int actual_height = hX - 2 * padding;
            int actual_bottom = actual_top + actual_height;
            int actual_right = actual_left + actual_width;
            float maxY = GetMaxY(copy_points, n);
            int grid_size = 11;
            Pen GridPen = new Pen(Color.Gray, 1f);
            PointF K1, K2, K3, K4;
            for (double i = 0.5; i < grid_size; i += 1.0)
            {
                //вертикальная
                K1 = new PointF((float)(actual_left + i * actual_width / grid_size), actual_top);
                K2 = new PointF((float)(actual_left + i * actual_width / grid_size), actual_bottom);
                gr.DrawLine(GridPen, K1, K2);
                double v = 0 + i * (toX - 0) / grid_size;
                string s1 = v.ToString("0.00");
                gr.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width / grid_size, actual_bottom + 0);


                K3 = new PointF(actual_left, (float)(actual_top + i * actual_height / grid_size));
                K4 = new PointF(actual_right, (float)(actual_top + i * actual_height / grid_size));
                gr.DrawLine(GridPen, K3, K4);
                double g = 0 + i * (double)(maxY / grid_size);
                string s2 = g.ToString("0.00");
                gr.DrawString(s2, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding, actual_bottom - (float)i * actual_height / grid_size);
            }

        }
        static public void PainGraph(Graphics gr, PictureBox pictureBox, Pen penG, PointF[] points, double toX, int n)//Отрисовка графика
        {


            PointF[] copy_points = new PointF[n];
            copy_points = (PointF[])points.Clone();

            int wX, hX;
            wX = pictureBox.Width;
            hX = pictureBox.Height;
            int actual_width = wX - 2 * padding - left_keys_padding;
            int actual_height = hX - 2 * padding;
            int actual_bottom = actual_top + actual_height;
            int actual_right = actual_left + actual_width;
            float maxY = GetMaxY(copy_points, n); ;
            PointF actual_tb = new PointF(actual_top, actual_bottom);//для y
            PointF actual_rl = new PointF(actual_right, actual_left);//для x
            PointF from_toX = new PointF(0, (float)(toX));
            PointF from_toY = new PointF(0, maxY * (float)1.2);
            convert_range_graph(copy_points, actual_rl, actual_tb, from_toX, from_toY);
            gr.DrawLines(penG, copy_points);
        }
        static public float GetMaxY(PointF[] points, int n)
        {
            float m = 0;
            for (int i = 0; i < n; i++)
            {
                if (m < Math.Abs(points[i].Y)) m = Math.Abs(points[i].Y);//макс значение Y

            }
            return m;
        }
        static public void convert_range_graph(PointF[] data, PointF actual_rl, PointF actual_tb, PointF from_toX, PointF from_toY)
        {
            //actual-размер:X-top/right Y-right,left
            //from_to: X-мин, Y-макс
            float kx = (actual_rl.X - actual_rl.Y) / (from_toX.Y - from_toX.X);
            float ky = (actual_tb.X - actual_tb.Y) / (from_toY.Y - from_toY.X);
            for (int i = 0; i < data.Length; i++)
            {
                data[i].X = (data[i].X - from_toX.X) * kx + actual_rl.Y;
                data[i].Y = (data[i].Y - from_toY.X) * ky + actual_tb.Y;
            }
        }



        //class Matrix
        //{
        //    // Скрытые поля
        //    private int n;
        //    private int[,] mass;

        //    // Создаем конструкторы матрицы
        //    public Matrix() { }
        //    public int N
        //    {
        //        get { return n; }
        //        set { if (value > 0) n = value; }
        //    }

        //    // Задаем аксессоры для работы с полями вне класса Matrix
        //    public Matrix(int n)
        //    {
        //        this.n = n;
        //        mass = new int[this.n, this.n];
        //    }
        //    public int this[int i, int j]
        //    {
        //        get
        //        {
        //            return mass[i, j];
        //        }
        //        set
        //        {
        //            mass[i, j] = value;
        //        }
        //    }

        //    // Ввод матрицы с клавиатуры
        //    public void WriteMat()
        //    {
        //        for (int i = 0; i < n; i++)
        //        {
        //            for (int j = 0; j < n; j++)
        //            {
        //                Console.WriteLine("Введите элемент матрицы {0}:{1}", i + 1, j + 1);
        //                mass[i, j] = Convert.ToInt32(Console.ReadLine());
        //            }
        //        }
        //    }

        //    // Вывод матрицы с клавиатуры
        //    public void ReadMat()
        //    {
        //        for (int i = 0; i < n; i++)
        //        {
        //            for (int j = 0; j < n; j++)
        //            {
        //                Console.Write(mass[i, j] + "\t");
        //            }
        //            Console.WriteLine();
        //        }
        //    }


        //    // Проверка матрицы А на единичность
        //    public void oneMat(Matrix a)
        //    {
        //        int count = 0;
        //        for (int i = 0; i < n; i++)
        //        {
        //            for (int j = 0; j < n; j++)
        //            {
        //                if (a[i, j] == 1 && i == j)
        //                {
        //                    count++;
        //                }
        //            }

        //        }
        //        if (count == a.N)
        //        {
        //            Console.WriteLine("Единичная");
        //        }
        //        else Console.WriteLine("Не единичная");
        //    }


        //    // Умножение матрицы А на число
        //    public static Matrix umnch(Matrix a, int ch)
        //    {
        //        Matrix resMass = new Matrix(a.N);
        //        for (int i = 0; i < a.N; i++)
        //        {
        //            for (int j = 0; j < a.N; j++)
        //            {
        //                resMass[i, j] = a[i, j] * ch;
        //            }
        //        }
        //        return resMass;
        //    }

        //    // Умножение матрицы А на матрицу Б
        //    public static Matrix umn(Matrix a, Matrix b)
        //    {
        //        Matrix resMass = new Matrix(a.N);
        //        for (int i = 0; i < a.N; i++)
        //            for (int j = 0; j < b.N; j++)
        //                for (int k = 0; k < b.N; k++)
        //                    resMass[i, j] += a[i, k] * b[k, j];

        //        return resMass;
        //    }



        //    // перегрузка оператора умножения
        //    public static Matrix operator *(Matrix a, Matrix b)
        //    {
        //        return Matrix.umn(a, b);
        //    }

        //    public static Matrix operator *(Matrix a, int b)
        //    {
        //        return Matrix.umnch(a, b);
        //    }


        //    // Метод вычитания матрицы Б из матрицы А
        //    public static Matrix razn(Matrix a, Matrix b)
        //    {
        //        Matrix resMass = new Matrix(a.N);
        //        for (int i = 0; i < a.N; i++)
        //        {
        //            for (int j = 0; j < b.N; j++)
        //            {
        //                resMass[i, j] = a[i, j] - b[i, j];
        //            }
        //        }
        //        return resMass;
        //    }

        //    // Перегрузка оператора вычитания
        //    public static Matrix operator -(Matrix a, Matrix b)
        //    {
        //        return Matrix.razn(a, b);
        //    }
        //    public static Matrix Sum(Matrix a, Matrix b)
        //    {
        //        Matrix resMass = new Matrix(a.N);
        //        for (int i = 0; i < a.N; i++)
        //        {
        //            for (int j = 0; j < b.N; j++)
        //            {
        //                resMass[i, j] = a[i, j] + b[i, j];
        //            }
        //        }
        //        return resMass;
        //    }
        //    // Перегрузка сложения
        //    public static Matrix operator +(Matrix a, Matrix b)
        //    {
        //        return Matrix.Sum(a, b);
        //    }
        //    // Деструктор Matrix
        //    ~Matrix()
        //    {
        //        Console.WriteLine("Очистка");
        //    }

    }
}
