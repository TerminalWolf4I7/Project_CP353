CREATE TABLE "users"(
    "user_id" SERIAL NOT NULL,
    "name" VARCHAR(255) NULL,
    "role" VARCHAR(50) NULL
);
ALTER TABLE
    "users" ADD PRIMARY KEY("user_id");
CREATE TABLE "restaurants"(
    "restaurant_id" SERIAL NOT NULL,
    "user_id" INTEGER NULL,
    "name" VARCHAR(255) NULL,
    "address" TEXT NULL,
    "phone" VARCHAR(50) NULL
);
ALTER TABLE
    "restaurants" ADD PRIMARY KEY("restaurant_id");
ALTER TABLE
    "restaurants" ADD CONSTRAINT "restaurants_user_id_unique" UNIQUE("user_id");
CREATE TABLE "riders"(
    "rider_id" SERIAL NOT NULL,
    "user_id" INTEGER NULL,
    "vehicle_type" VARCHAR(100) NULL,
    "status" VARCHAR(50) NULL
);
ALTER TABLE
    "riders" ADD PRIMARY KEY("rider_id");
ALTER TABLE
    "riders" ADD CONSTRAINT "riders_user_id_unique" UNIQUE("user_id");
CREATE TABLE "menu_items"(
    "item_id" SERIAL NOT NULL,
    "restaurant_id" INTEGER NULL,
    "name" VARCHAR(255) NULL,
    "price" DECIMAL(10, 2) NULL,
    "description" TEXT NULL,
    "is_available" BOOLEAN NULL DEFAULT TRUE
);
ALTER TABLE
    "menu_items" ADD PRIMARY KEY("item_id");
CREATE TABLE "carts"(
    "cart_id" SERIAL NOT NULL,
    "user_id" INTEGER NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NULL DEFAULT CURRENT_TIMESTAMP
);
ALTER TABLE
    "carts" ADD PRIMARY KEY("cart_id");
CREATE TABLE "cart_items"(
    "cart_item_id" SERIAL NOT NULL,
    "cart_id" INTEGER NULL,
    "item_id" INTEGER NULL,
    "quantity" INTEGER NULL
);
ALTER TABLE
    "cart_items" ADD PRIMARY KEY("cart_item_id");
CREATE TABLE "orders"(
    "order_id" SERIAL NOT NULL,
    "user_id" INTEGER NULL,
    "restaurant_id" INTEGER NULL,
    "rider_id" INTEGER NULL,
    "total_price" DECIMAL(10, 2) NULL,
    "status" VARCHAR(50) NULL,
    "created_at" TIMESTAMP(0) WITHOUT TIME ZONE NULL DEFAULT CURRENT_TIMESTAMP
);
ALTER TABLE
    "orders" ADD PRIMARY KEY("order_id");
CREATE TABLE "order_items"(
    "order_item_id" SERIAL NOT NULL,
    "order_id" INTEGER NULL,
    "item_id" INTEGER NULL,
    "quantity" INTEGER NULL,
    "price" DECIMAL(10, 2) NULL
);
ALTER TABLE
    "order_items" ADD PRIMARY KEY("order_item_id");
CREATE TABLE "payments"(
    "payment_id" SERIAL NOT NULL,
    "order_id" INTEGER NULL,
    "amount" DECIMAL(10, 2) NULL,
    "method" VARCHAR(50) NULL,
    "status" VARCHAR(50) NULL,
    "paid_at" TIMESTAMP(0) WITHOUT TIME ZONE NULL
);
ALTER TABLE
    "payments" ADD PRIMARY KEY("payment_id");
ALTER TABLE
    "payments" ADD CONSTRAINT "payments_order_id_unique" UNIQUE("order_id");
CREATE TABLE "deliveries"(
    "delivery_id" SERIAL NOT NULL,
    "order_id" INTEGER NULL,
    "rider_id" INTEGER NULL,
    "pickup_time" TIMESTAMP(0) WITHOUT TIME ZONE NULL,
    "delivered_time" TIMESTAMP(0) WITHOUT TIME ZONE NULL,
    "status" VARCHAR(50) NULL
);
ALTER TABLE
    "deliveries" ADD PRIMARY KEY("delivery_id");
ALTER TABLE
    "deliveries" ADD CONSTRAINT "deliveries_order_id_unique" UNIQUE("order_id");
ALTER TABLE
    "cart_items" ADD CONSTRAINT "cart_items_item_id_foreign" FOREIGN KEY("item_id") REFERENCES "menu_items"("item_id");
ALTER TABLE
    "cart_items" ADD CONSTRAINT "cart_items_cart_id_foreign" FOREIGN KEY("cart_id") REFERENCES "carts"("cart_id");
ALTER TABLE
    "deliveries" ADD CONSTRAINT "deliveries_rider_id_foreign" FOREIGN KEY("rider_id") REFERENCES "riders"("rider_id");
ALTER TABLE
    "orders" ADD CONSTRAINT "orders_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "users"("user_id");
ALTER TABLE
    "orders" ADD CONSTRAINT "orders_restaurant_id_foreign" FOREIGN KEY("restaurant_id") REFERENCES "restaurants"("restaurant_id");
ALTER TABLE
    "orders" ADD CONSTRAINT "orders_rider_id_foreign" FOREIGN KEY("rider_id") REFERENCES "riders"("rider_id");
ALTER TABLE
    "order_items" ADD CONSTRAINT "order_items_order_id_foreign" FOREIGN KEY("order_id") REFERENCES "orders"("order_id");
ALTER TABLE
    "order_items" ADD CONSTRAINT "order_items_item_id_foreign" FOREIGN KEY("item_id") REFERENCES "menu_items"("item_id");
ALTER TABLE
    "payments" ADD CONSTRAINT "payments_order_id_foreign" FOREIGN KEY("order_id") REFERENCES "orders"("order_id");
ALTER TABLE
    "deliveries" ADD CONSTRAINT "deliveries_order_id_foreign" FOREIGN KEY("order_id") REFERENCES "orders"("order_id");
ALTER TABLE
    "restaurants" ADD CONSTRAINT "restaurants_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "users"("user_id");
ALTER TABLE
    "riders" ADD CONSTRAINT "riders_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "users"("user_id");
ALTER TABLE
    "menu_items" ADD CONSTRAINT "menu_items_restaurant_id_foreign" FOREIGN KEY("restaurant_id") REFERENCES "restaurants"("restaurant_id");
ALTER TABLE
    "carts" ADD CONSTRAINT "carts_user_id_foreign" FOREIGN KEY("user_id") REFERENCES "users"("user_id");
