CREATE TABLE customers (
    id SERIAL PRIMARY KEY,
    "guid" UUID UNIQUE NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email_address VARCHAR(100) UNIQUE NOT NULL,
    mobile_number VARCHAR(20) NOT NULL
);

CREATE TABLE email_templates (
    "name" VARCHAR(50) PRIMARY KEY,
    "subject" VARCHAR(1000) NOT NULL,
    "body" TEXT NOT NULL
);

CREATE TABLE sms_templates (
    "name" VARCHAR(50) PRIMARY KEY,
    "message" VARCHAR(160) NOT NULL
);

COPY customers
FROM '/docker-entrypoint-initdb.d/docker-entrypoint-initdb.d/Customers.csv'
DELIMITER ','
CSV HEADER;

SELECT setval(pg_get_serial_sequence('customers', 'id'), (SELECT MAX(id) FROM customers) + 1);

COPY email_templates
FROM '/docker-entrypoint-initdb.d/docker-entrypoint-initdb.d/EmailTemplates.csv'
DELIMITER ','
CSV HEADER;

COPY sms_templates
FROM '/docker-entrypoint-initdb.d/docker-entrypoint-initdb.d/SmsTemplates.csv'
DELIMITER ','
CSV HEADER;