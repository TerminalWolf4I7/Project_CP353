-- ===========================================================================
-- Script สำหรับสร้างโครงสร้างฐานข้อมูล (Database Schema) ของระบบ Delivery
-- สำหรับใช้งานกับ PostgreSQL
-- ===========================================================================

-- 1. ล้างตารางเก่าออกทั้งหมด (ถ้ามี) เพื่อเริ่มสร้างใหม่จากศูนย์
-- เรียงลำดับตามความสัมพันธ์ (ลบตารางที่มี Foreign Key ก่อน)
DROP TABLE IF EXISTS "order_items" CASCADE;
DROP TABLE IF EXISTS "orders" CASCADE;
DROP TABLE IF EXISTS "cart_items" CASCADE;
DROP TABLE IF EXISTS "carts" CASCADE;
DROP TABLE IF EXISTS "menu_items" CASCADE;
DROP TABLE IF EXISTS "riders" CASCADE;
DROP TABLE IF EXISTS "restaurants" CASCADE;
DROP TABLE IF EXISTS "users" CASCADE;

-- 2. สร้างตาราง Users: เก็บข้อมูลผู้ใช้ทุกคนในระบบ
CREATE TABLE "users" (
    "user_id" SERIAL NOT NULL,           -- รหัสผู้ใช้ (Auto Increment)
    "name" VARCHAR(255) NOT NULL,        -- ชื่อผู้ใช้
    "role" VARCHAR(50) NOT NULL,         -- บทบาท (customer, restaurant, rider)
    CONSTRAINT users_pkey PRIMARY KEY ("user_id")
);

-- 3. สร้างตาราง Restaurants: เก็บข้อมูลร้านอาหาร
CREATE TABLE "restaurants" (
    "restaurant_id" SERIAL NOT NULL,
    "user_id" INTEGER NOT NULL,          -- เชื่อมกับเจ้าของร้านในตาราง Users
    "name" VARCHAR(255) NOT NULL,
    "address" TEXT,
    "phone" VARCHAR(50),
    CONSTRAINT restaurants_pkey PRIMARY KEY ("restaurant_id"),
    CONSTRAINT restaurants_user_id_unique UNIQUE ("user_id"), -- 1 User มีได้ 1 ร้าน
    CONSTRAINT restaurants_user_id_fk
        FOREIGN KEY ("user_id")
        REFERENCES "users"("user_id")
);

-- 4. สร้างตาราง Riders: เก็บข้อมูลคนส่งอาหาร
CREATE TABLE "riders" (
    "rider_id" SERIAL NOT NULL,
    "user_id" INTEGER NOT NULL,          -- เชื่อมกับตาราง Users
    "vehicle_type" VARCHAR(100),         -- ประเภทรถ
    "status" VARCHAR(50) NOT NULL,       -- สถานะการทำงาน (available, busy)
    CONSTRAINT riders_pkey PRIMARY KEY ("rider_id"),
    CONSTRAINT riders_user_id_unique UNIQUE ("user_id"),
    CONSTRAINT riders_user_id_fk
        FOREIGN KEY ("user_id")
        REFERENCES "users"("user_id")
);

-- 5. สร้างตาราง Menu_Items: รายการอาหารของแต่ละร้าน
CREATE TABLE "menu_items" (
    "item_id" SERIAL NOT NULL,
    "restaurant_id" INTEGER NOT NULL,    -- สังกัดร้านอาหารใด
    "name" VARCHAR(255) NOT NULL,
    "price" DECIMAL(10,2) NOT NULL,      -- ราคาอาหาร
    "description" TEXT,
    "is_available" BOOLEAN DEFAULT TRUE, -- พร้อมขายหรือไม่
    CONSTRAINT menu_items_pkey PRIMARY KEY ("item_id"),
    CONSTRAINT menu_items_restaurant_fk
        FOREIGN KEY ("restaurant_id")
        REFERENCES "restaurants"("restaurant_id")
);

-- 6. สร้างตาราง Carts: ตะกร้าสินค้าชั่วคราวของผู้ใช้
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

-- 7. สร้างตาราง Cart_Items: รายการสินค้าที่อยู่ในตะกร้า
CREATE TABLE "cart_items" (
    "cart_item_id" SERIAL NOT NULL,
    "cart_id" INTEGER NOT NULL,
    "item_id" INTEGER NOT NULL,
    "quantity" INTEGER NOT NULL,         -- จำนวนที่เลือก
    CONSTRAINT cart_items_pkey PRIMARY KEY ("cart_item_id"),
    CONSTRAINT cart_items_cart_fk
        FOREIGN KEY ("cart_id")
        REFERENCES "carts"("cart_id"),
    CONSTRAINT cart_items_item_fk
        FOREIGN KEY ("item_id")
        REFERENCES "menu_items"("item_id")
);

-- 8. สร้างตาราง Orders: ข้อมูลการสั่งซื้อที่สำเร็จแล้ว
CREATE TABLE "orders" (
    "order_id" SERIAL NOT NULL,
    "user_id" INTEGER NOT NULL,          -- ลูกค้าที่สั่ง
    "restaurant_id" INTEGER NOT NULL,    -- สั่งจากร้านใด
    "rider_id" INTEGER,                  -- ไรเดอร์คนไหนเป็นผู้ส่ง (Null ได้ถ้ายังไม่มีคนรับ)
    "total_price" DECIMAL(10,2) NOT NULL, -- ยอดรวมราคาทั้งหมด
    "status" VARCHAR(50) NOT NULL,       -- สถานะออเดอร์ (pending, cooking, delivering, completed)
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

-- 9. สร้างตาราง Order_Items: รายการอาหารที่ถูกสั่งจริง (เก็บราคา ณ ขณะที่สั่ง)
CREATE TABLE "order_items" (
    "order_item_id" SERIAL NOT NULL,
    "order_id" INTEGER NOT NULL,
    "item_id" INTEGER NOT NULL,
    "quantity" INTEGER NOT NULL,
    "price" DECIMAL(10,2) NOT NULL,      -- ราคา ณ วันที่สั่งซื้อ
    CONSTRAINT order_items_pkey PRIMARY KEY ("order_item_id"),
    CONSTRAINT order_items_order_fk
        FOREIGN KEY ("order_id")
        REFERENCES "orders"("order_id"),
    CONSTRAINT order_items_item_fk
        FOREIGN KEY ("item_id")
        REFERENCES "menu_items"("item_id")
);

-- 10. เพิ่มข้อกำหนด (Constraints) เพื่อความถูกต้องของข้อมูล (Data Integrity)
ALTER TABLE "cart_items" ADD CONSTRAINT cart_items_quantity_check CHECK ("quantity" > 0);
ALTER TABLE "order_items" ADD CONSTRAINT order_items_quantity_check CHECK ("quantity" > 0);
ALTER TABLE "menu_items" ADD CONSTRAINT menu_items_price_check CHECK ("price" >= 0);
ALTER TABLE "order_items" ADD CONSTRAINT order_items_price_check CHECK ("price" >= 0);
ALTER TABLE "orders" ADD CONSTRAINT orders_total_price_check CHECK ("total_price" >= 0);