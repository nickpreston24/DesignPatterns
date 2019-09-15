namespace ExampleMocking.TestClasses
{
    public class CandyShop : ICandyShop
    {
        public CandyShop()
        {
        }

        public void BuyCandy(ICandy lollipop)
        {
            //throw new NotImplementedException();
        }

        public ICandy GetLollipop()
        {
            return new Lollipop();
        }

        public ICandy GetTopSellingCandy()
        {
            var lolli = new Lollipop();
            BuyCandy(lolli);
            return lolli;
        }
    }

    public class SweetTooth : IPerson
    {
        public SweetTooth()
        {
        }

        public void BuyTastiestCandy(ICandyShop shop)
        {
            shop.BuyCandy(shop.GetLollipop());
        }
    }
}