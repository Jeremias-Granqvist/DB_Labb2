namespace DB_Labb2.Model
{
    public class InventoryManager : ModelBase
    {
        public event EventHandler<Inventory> InventoryAdded;
        public event EventHandler<Inventory> InventoryEdited;
        public event EventHandler<Inventory> InventoryDeleted;

        public void AddInventory(Inventory inventory)
        {
            using (var context = new BookstoreContext())
            {
                var existingInventory = context.Inventory.FirstOrDefault(i => i.InventoryISBN13 == inventory.InventoryISBN13 && i.StoreID == inventory.StoreID);
                if (existingInventory != null)
                {
                    existingInventory.Amount = inventory.Amount;
                }
                else
                {
                    context.Inventory.Add(inventory);
                }
                context.SaveChanges();

            }
            OnInventoryAdded(inventory);
        }

        public void EditInventory(Inventory updatedInventory)
        {
            using (var context = new BookstoreContext())
            {
                var existingInventory = context.Inventory.FirstOrDefault(i => i.InventoryISBN13 == updatedInventory.InventoryISBN13 && i.StoreID == updatedInventory.StoreID);
                if (existingInventory != null)
                {
                    existingInventory.Amount = updatedInventory.Amount;
                    context.SaveChanges();

                    if (existingInventory.Amount == 0)
                    {
                        OnInventoryDeleted(updatedInventory);
                    }
                    else
                    {
                        OnInventoryEdited(updatedInventory);

                    }

                }
            }
        }

        public void DeleteInventory(Inventory inventory)
        {
            using (var context = new BookstoreContext())
            {
                var inventoryToDelete = context.Inventory.FirstOrDefault(i => i.InventoryISBN13 == inventory.InventoryISBN13 && i.StoreID == inventory.StoreID);
                if (inventoryToDelete != null)
                {
                    context.SaveChanges();
                }
            }
            OnInventoryAdded(inventory);
        }

        protected virtual void OnInventoryAdded(Inventory inventory)
        {
            InventoryAdded?.Invoke(this, inventory);
        }
        protected virtual void OnInventoryEdited(Inventory inventory)
        {
            InventoryEdited?.Invoke(this, inventory);
        }
        protected virtual void OnInventoryDeleted(Inventory inventory)
        {
            InventoryDeleted?.Invoke(this, inventory);
        }

    }
}
