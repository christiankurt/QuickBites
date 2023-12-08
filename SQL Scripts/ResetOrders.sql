/* ALSO RESETS QUEUE NUMBER */

DROP TABLE Orders;

CREATE TABLE Orders (
	queue_number INT PRIMARY KEY IDENTITY,
	order_date DATE,
	customer_id INT,
	order_item NVARCHAR(255),
	order_quantity INT,
	quantified_price FLOAT,
	FOREIGN KEY (customer_id) REFERENCES UserAccounts(id),
	FOREIGN KEY (order_item) REFERENCES Products(item)
);
