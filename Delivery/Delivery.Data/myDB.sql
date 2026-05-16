--ล้าง BD เก่าออกหมด
DROP TABLE IF EXISTS "order_items" CASCADE;
DROP TABLE IF EXISTS "orders" CASCADE;
DROP TABLE IF EXISTS "cart_items" CASCADE;
DROP TABLE IF EXISTS "carts" CASCADE;
DROP TABLE IF EXISTS "menu_items" CASCADE;
DROP TABLE IF EXISTS "riders" CASCADE;
DROP TABLE IF EXISTS "restaurants" CASCADE;
DROP TABLE IF EXISTS "users" CASCADE;

CREATE TABLE "users" (
    "user_id" SERIAL NOT NULL,
    "name" VARCHAR(255) NOT NULL,
    "role" VARCHAR(50) NOT NULL,
    CONSTRAINT users_pkey PRIMARY KEY ("user_id")
);

CREATE TABLE "restaurants" (
    "restaurant_id" SERIAL NOT NULL,
    "user_id" INTEGER NOT NULL,
    "name" VARCHAR(255) NOT NULL,
    "address" TEXT,
    "phone" VARCHAR(50),
    CONSTRAINT restaurants_pkey PRIMARY KEY ("restaurant_id"),
    CONSTRAINT restaurants_user_id_unique UNIQUE ("user_id"),
    CONSTRAINT restaurants_user_id_fk
        FOREIGN KEY ("user_id")
        REFERENCES "users"("user_id")
);

CREATE TABLE "riders" (
    "rider_id" SERIAL NOT NULL,
    "user_id" INTEGER NOT NULL,
    "vehicle_type" VARCHAR(100),
    "status" VARCHAR(50) NOT NULL,
    CONSTRAINT riders_pkey PRIMARY KEY ("rider_id"),
    CONSTRAINT riders_user_id_unique UNIQUE ("user_id"),
    CONSTRAINT riders_user_id_fk
        FOREIGN KEY ("user_id")
        REFERENCES "users"("user_id")
);

CREATE TABLE "menu_items" (
    "item_id" SERIAL NOT NULL,
    "restaurant_id" INTEGER NOT NULL,
    "name" VARCHAR(255) NOT NULL,
    "price" DECIMAL(10,2) NOT NULL,
    "description" TEXT,
    "is_available" BOOLEAN DEFAULT TRUE,
    CONSTRAINT menu_items_pkey PRIMARY KEY ("item_id"),
    CONSTRAINT menu_items_restaurant_fk
        FOREIGN KEY ("restaurant_id")
        REFERENCES "restaurants"("restaurant_id")
);

CREATE TABLE "carts" (
    "cart_id" SERIAL NOT NULL,
    "user_id" INTEGER NOT NULL,
    "restaurant_id" INTEGER NOT NULL,
    "created_at" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT carts_pkey PRIMARY KEY ("cart_id"),
    CONSTRAINT carts_user_id_fk
        FOREIGN KEY ("user_id")
        REFERENCES "users"("user_id"),
    CONSTRAINT carts_restaurant_fk
        FOREIGN KEY ("restaurant_id")
        REFERENCES "restaurants"("restaurant_id")
);

CREATE TABLE "cart_items" (
    "cart_item_id" SERIAL NOT NULL,
    "cart_id" INTEGER NOT NULL,
    "item_id" INTEGER NOT NULL,
    "quantity" INTEGER NOT NULL,
    CONSTRAINT cart_items_pkey PRIMARY KEY ("cart_item_id"),
    CONSTRAINT cart_items_cart_fk
        FOREIGN KEY ("cart_id")
        REFERENCES "carts"("cart_id"),
    CONSTRAINT cart_items_item_fk
        FOREIGN KEY ("item_id")
        REFERENCES "menu_items"("item_id")
);

CREATE TABLE "orders" (
    "order_id" SERIAL NOT NULL,
    "user_id" INTEGER NOT NULL,
    "restaurant_id" INTEGER NOT NULL,
    "rider_id" INTEGER,
    "total_price" DECIMAL(10,2) NOT NULL,
    "status" VARCHAR(50) NOT NULL,
    "created_at" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT orders_pkey PRIMARY KEY ("order_id"),
    CONSTRAINT orders_user_fk
        FOREIGN KEY ("user_id")
        REFERENCES "users"("user_id"),
    CONSTRAINT orders_restaurant_fk
        FOREIGN KEY ("restaurant_id")
        REFERENCES "restaurants"("restaurant_id"),
    CONSTRAINT orders_rider_fk
        FOREIGN KEY ("rider_id")
        REFERENCES "riders"("rider_id")
);

CREATE TABLE "order_items" (
    "order_item_id" SERIAL NOT NULL,
    "order_id" INTEGER NOT NULL,
    "item_id" INTEGER NOT NULL,
    "quantity" INTEGER NOT NULL,
    "price" DECIMAL(10,2) NOT NULL,
    CONSTRAINT order_items_pkey PRIMARY KEY ("order_item_id"),
    CONSTRAINT order_items_order_fk
        FOREIGN KEY ("order_id")
        REFERENCES "orders"("order_id"),
    CONSTRAINT order_items_item_fk
        FOREIGN KEY ("item_id")
        REFERENCES "menu_items"("item_id")
);

ALTER TABLE "cart_items"
ADD CONSTRAINT cart_items_quantity_check
CHECK ("quantity" > 0);

ALTER TABLE "order_items"
ADD CONSTRAINT order_items_quantity_check
CHECK ("quantity" > 0);

ALTER TABLE "menu_items"
ADD CONSTRAINT menu_items_price_check
CHECK ("price" >= 0);

ALTER TABLE "order_items"
ADD CONSTRAINT order_items_price_check
CHECK ("price" >= 0);

ALTER TABLE "orders"
ADD CONSTRAINT orders_total_price_check
CHECK ("total_price" >= 0);