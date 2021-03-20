using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace NezarkaBookstoreWeb {
	
	class Program {
		private static void Main(string[] args) {
			//var reader = Console.In;
			var reader = new StreamReader(args[0]);

			var store = ModelStore.LoadFrom(reader);
			if (store == null) {
				Console.WriteLine("Data error.");
				return;
			}

			var masterController = new MasterController(
				store,
				new BooksController(
					store,
					new BooksViewFactory(),
					new BooksDetailViewFactory()
				),
				new ShoppingCartController(
					store,
					new ShoppingCartViewFactory()
				),
				new MasterViewFactory(),
				new MenuViewFactory()
			);

			var writer = Console.Out;

			string line;
			while ((line = reader.ReadLine()) != null) {
				masterController.ExecuteRequest(line, writer);
				writer.WriteLine("====");
			}
		}
	}

}
