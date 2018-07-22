﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TShirtPicker.Data;
using TShirtPicker.Data.Models;

namespace TShirtPicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TShirtRepository repository = new TShirtRepository();
        private Log log = new Log();

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                this.GetRandomTShirt();
            }
            catch (Exception exception)
            {
                this.log.LogData(exception.Message, Severity.Error);
            }
            finally
            {
                MessageBox.Show("A fatal error occured.");
            }
        }

        public void GetRandomTShirt()
        {
            Random rnd = new Random();
            List<TShirt> tShirts = this.repository.GetAll().ToList();

            string blackTShirtsMessage = string.Format(
                "{0} black T-shirts availabe.", tShirts.Count(x => x.Color.Contains("Black")));
            string whiteTShirtsMessage = string.Format(
                "{0} white T-shirts available.", tShirts.Count(x => x.Color.Contains("White")));
            string greyTShirstMessage = string.Format(
                "{0} grey T-shirts available", tShirts.Count(x => x.Color.Contains("Grey")));
            string navyTShirtsMessage = string.Format(
                "{0} grey T-shirts available", tShirts.Count(x => x.Color.Contains("Navy")));

            this.log.LogData(blackTShirtsMessage, Severity.Information);
            this.log.LogData(whiteTShirtsMessage, Severity.Information);
            this.log.LogData(greyTShirstMessage, Severity.Information);
            this.log.LogData(navyTShirtsMessage, Severity.Information);

            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday || tShirts.All(x => x.Quantity <= 0))
            {
                this.RestockTShirts(tShirts);
            }

            int loopCounter = 0;

            while (true)
            {
                loopCounter++;
                int index = rnd.Next(0, tShirts.Count);

                this.log.LogData($"Loop number {loopCounter}, Number {index}", Severity.Information);
                this.log.LogData($"{tShirts[index].Color} chosen", Severity.Information);

                if (tShirts[index].Quantity > 0)
                {
                    ImageSource imageSource = new BitmapImage(
                        new Uri($@"../../Resources/{tShirts[index].Color}.jpg", UriKind.Relative));
                    this.Image.Source = imageSource;
                    this.repository.DecreaseQuantity(tShirts[index].Id, tShirts[index].Quantity - 1);
                    break;
                }
            }

        }

        private void RestockTShirts(List<TShirt> tShirts)
        {
            foreach (var tShirt in tShirts)
            {
                if (tShirt.Color == "61422-30 White")
                {
                    this.repository.Restock(1, tShirt.Color);
                    tShirt.Quantity = 1;
                }
                else if (tShirt.Color == "61422-32 Navy")
                {
                    this.repository.Restock(4, tShirt.Color);
                    tShirt.Quantity = 4;
                }
                else if (tShirt.Color == "61422-36 Black")
                {
                    this.repository.Restock(1, tShirt.Color);
                    tShirt.Quantity = 1;
                }
                else if (tShirt.Color == "61422-94 Heather Grey")
                {
                    this.repository.Restock(1, tShirt.Color);
                    tShirt.Quantity = 1;
                }
            }
        }
    }
}