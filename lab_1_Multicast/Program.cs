class Program {
    static void Main(string[] args) {
        const int SEND_LIFE_TIME = 1000;
        const int PRINT_LIFE_TIME = 4000;

        Searcher searcher = new Searcher();
        Timer sendTimer = new Timer((object? obj) => {searcher.Send();}, null, 0, SEND_LIFE_TIME);
        Timer printTimer = new Timer((object? obj) => {searcher.PrintAliveCopies(); }, null, 0, PRINT_LIFE_TIME);
        searcher.GetCopies();
        while (true) 
        { 
            Thread.Sleep(1000); 
        }
    }
}