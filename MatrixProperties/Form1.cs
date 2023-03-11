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
        private int M;
        private double[,] A1;
        private double[,] A2;
        private void Form1_Load(object sender, EventArgs e)
        {
            for_M.Text = "3";
        }
        public Form1()
        {
            InitializeComponent();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;

        }

        private void button1_Click(object sender, EventArgs e)//построение обратной матрицы
        {
            M = Convert.ToInt32(for_M.Text);
            A1 = new double[M,M];
            double det=0;//определитель матрицы
            while(det==0)
            {
                Random rnd = new Random();
                for(int i=0;i<M;i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        A1[i,j] = (rnd.NextDouble() - 1);
                    }
                }
                det=Determ(A1); 
            }
            for (int row = 0; row < A1.GetLength(0); row++)
            {
                for (int column = 0; column < A1.GetLength(1); column++)
                    Console.Write(A1[row, column] + "\t");
                Console.WriteLine();
            }

        }

        private void button2_Click(object sender, EventArgs e)//разложение обратной матрицы
        {

        }

        private void button3_Click(object sender, EventArgs e)//свойства обратной матрицы
        {

        }

        private void button5_Click(object sender, EventArgs e)//построение псевдообратной матрицы
        {
            M = Convert.ToInt32(for_M.Text);
            A2 = new double[M, M];
        }

        private void button6_Click(object sender, EventArgs e)//разложение псевдообратной матрицы
        {

        }

        private void button7_Click(object sender, EventArgs e)//свойства псевдообратной матрицы
        {

        }
        private void button4_Click(object sender, EventArgs e)//ок
        {

        }
        //для вычисления определителя
        public static double[,] GetMinor(double[,] matrix, int row, int column)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) throw new Exception(" Число строк в матрице не совпадает с числом столбцов");
            double[,] buf = new double[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if ((i != row) || (j != column))
                    {
                        if (i > row && j < column) buf[i - 1, j] = matrix[i, j];
                        if (i < row && j > column) buf[i, j - 1] = matrix[i, j];
                        if (i > row && j > column) buf[i - 1, j - 1] = matrix[i, j];
                        if (i < row && j < column) buf[i, j] = matrix[i, j];
                    }
                }
            return buf;
        }
        public static double Determ(double[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1)) throw new Exception(" Число строк в матрице не совпадает с числом столбцов");
            double det = 0;
            int Rank = matrix.GetLength(0);
            if (Rank == 1) det = matrix[0, 0];
            if (Rank == 2) det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            if (Rank > 2)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    det += Math.Pow(-1, 0 + j) * matrix[0, j] * Determ(GetMinor(matrix, 0, j));
                }
            }
            return det;
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

    //}
}
