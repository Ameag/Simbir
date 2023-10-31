CREATE TABLE "Accounts" (
    "id" bigserial NOT NULL,
    "username" character varying NOT NULL UNIQUE,
    "password" character varying NOT NULL,
    "salt" character varying NOT NULL,
    "is_admin" BOOLEAN NOT NULL,
    "balance" int NOT NULL,
CONSTRAINT "Accounts_pk" PRIMARY KEY ("id")
);



CREATE TABLE "Transport" (
    "id" bigserial NOT NULL,
    "transport_type" character varying NOT NULL,
    "identifier" character varying NOT NULL,
    "can_be_rented" BOOLEAN NOT NULL,
    "model" character varying NOT NULL,
    "color" character varying NOT NULL,
    "description" TEXT,
    "latitude" float8 NOT NULL,
    "longitude" float8  NOT NULL,
    "minute_price" int,
    "day_price" int,
    "owner_id" int NOT NULL,
CONSTRAINT "Transport_pk" PRIMARY KEY ("id")
);



CREATE TABLE "Rent" (
    "id" serial NOT NULL,
    "user_id" int   NOT NULL,
    "transport_id" int NOT NULL,
    "lat" float8 ,
    "lon" float8 ,
    "radius" float8 ,
    "time_start" TIMESTAMP NOT NULL,
    "time_end" TIMESTAMP,
    "price_of_unit" float8 NOT NULL,
    "price_type" character varying NOT NULL,
    "final_price" int,
CONSTRAINT "Rent_pk" PRIMARY KEY ("id")
);

CREATE TABLE "BlackList"(
    "token" character varying NOT NULL,
    CONSTRAINT "BlackList_pk" PRIMARY KEY ("token")
);

ALTER TABLE "Rent" ADD CONSTRAINT "Rent_fk0" FOREIGN KEY ("user_id") REFERENCES "Accounts"("id");
ALTER TABLE "Rent" ADD CONSTRAINT "Rent_fk1" FOREIGN KEY ("transport_id") REFERENCES "Transport"("id");