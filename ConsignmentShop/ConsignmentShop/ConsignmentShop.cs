using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShop
{
    public partial class ConsignmentShop : Form
    {

        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();
        private List<Item> storeItems = new List<Item>();
        BindingSource itemsBinding = new BindingSource(); 
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();
        private decimal storeProfit = 0;

        public ConsignmentShop()
        {
            InitializeComponent();
            SetupData();
            BindingItems();
            CartBinding();
            VendorBinding();

        }
        public void SetupData()
        {
            store.Name = "Little Store";

            store.Vendors.Add(new Vendor { FirstName = "Bill", LastName = "Smith" });
            store.Vendors.Add(new Vendor { FirstName = "Sue", LastName = "Jones" });

            store.Items.Add(new Item
            {
                Title = "Moby Dick",
                Description = "A book about a whale",
                Price = 4.50M,
                Owner = store.Vendors[0],
                NumberOfBooks = 4
            });

            store.Items.Add(new Item
            {
                Title = "A Tale of Two Cities",
                Description = "A book about revolution",
                Price = 3.90M,
                Owner = store.Vendors[1],
                NumberOfBooks = 3
            });

            store.Items.Add(new Item
            {
                Title = "Harry Potter",
                Description = "A book about a magician",
                Price = 5.70M,
                Owner = store.Vendors[1],
                NumberOfBooks = 1
            });

            store.Items.Add(new Item
            {
                Title = "Jane Eyre",
                Description = " A book about a girl",
                Price = 1.15M,
                Owner = store.Vendors[0],
                NumberOfBooks = 2
            });

        }

        public void BindingItems()
        {
            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsListbox.DataSource = itemsBinding;
            itemsListbox.DisplayMember = "Display";
            itemsListbox.ValueMember = "Display";
        }
        public void CartBinding()
        {
            cartBinding.DataSource = shoppingCartData;
            shoppingCartListbox.DataSource = cartBinding;

            shoppingCartListbox.DisplayMember = "Display";
            shoppingCartListbox.ValueMember = "Display";
        }
        public void VendorBinding()
        {
            vendorsBinding.DataSource = store.Vendors;
            vendorListbox.DataSource = vendorsBinding;
            vendorListbox.DisplayMember = "Display";
            vendorListbox.ValueMember = "Display";
        }

        private void addToCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(itemsListbox.Text))
            {
                MessageBox.Show("THERE ARE NO MORE BOOKS AVAILABLE.");
            }
            else
            {
                int numOfBook = ((Item)itemsListbox.SelectedItem).NumberOfBooks;
                if (numOfBook > 0)
                {
                    ((Item)itemsListbox.SelectedItem).NumberOfBooks--;
                    ((Item)itemsListbox.SelectedItem).NumberOfBooksSold++;
                    Item selectedItem = (Item)itemsListbox.SelectedItem;
                    shoppingCartData.Add(selectedItem);
                    store.Items.Remove(selectedItem);
                    cartBinding.ResetBindings(false);
                }
                else
                    MessageBox.Show("The selected book is not available.");
            }
        }

        private void makePurchase_Click(object sender, EventArgs e)
        {
            foreach(Item item in shoppingCartData)
            {
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price;
                storeProfit += (1 - (decimal)item.Owner.Commission) * item.Price;
            }

            shoppingCartData.Clear();

            storeProfitValue.Text = string.Format(new CultureInfo("en-US"), "{0:C}", storeProfit);

            cartBinding.ResetBindings(false);
            itemsBinding.ResetBindings(false);
            vendorsBinding.ResetBindings(false);
        }

        private void returnButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(shoppingCartListbox.Text))
            {
                MessageBox.Show("THERE ARE NO BOOKS FOR RETURN.");
            }
            else
            {
                Item selectedItem = (Item)shoppingCartListbox.SelectedItem;
                ((Item)shoppingCartListbox.SelectedItem).NumberOfBooksSold--;
                ((Item)shoppingCartListbox.SelectedItem).NumberOfBooks++;
                shoppingCartData.Remove(selectedItem);
                storeItems.Add(selectedItem);
                cartBinding.ResetBindings(false);
            }
        }
    }
}
