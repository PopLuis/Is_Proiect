using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MultiTab
{
    public partial class CosWindow : Window
    {
        private Utilizator utilizator;

        public CosWindow(List<Produs> produse, Utilizator utilizator)
        {
            InitializeComponent();
            this.utilizator = utilizator;
            ListaCos.ItemsSource = produse;

            decimal total = produse.Sum(p => p.Pret * p.Cantitate);

            var categoriiPreasamblate = new List<string> { "laptop", "desktop", "imprimanta", "periferice" };
            bool continePreasamblate = produse.Any(p => categoriiPreasamblate.Contains(p.Categorie.ToLower()));

            if (continePreasamblate)
            {
                total += 100;
                TaxaTextBlock.Text = "Taxă suplimentară: 100 RON";
                TaxaTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                TaxaTextBlock.Visibility = Visibility.Collapsed;
            }

            TextTotal.Text = $"Total: {total} RON";
        }


        private void PlaseazaComanda_Click(object sender, RoutedEventArgs e)
        {
            var produseInCos = ListaCos.ItemsSource.Cast<Produs>().ToList();
            CheckoutWindow checkout = new CheckoutWindow(produseInCos, utilizator);
            bool? rezultat = checkout.ShowDialog();

            if (rezultat == true)
            {
                this.Close();
            }
        }

    }
}
