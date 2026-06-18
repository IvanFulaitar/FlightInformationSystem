DROP TRIGGER IF EXISTS trg_validate_flight_departure_date ON flights;
DROP FUNCTION IF EXISTS validate_flight_departure_date();
DROP TABLE IF EXISTS flights;

CREATE TABLE flights (
    flight_number VARCHAR(10) PRIMARY KEY,
    departure_datetime TIMESTAMP NOT NULL,
    departure_airport_city VARCHAR(100) NOT NULL,
    arrival_airport_city VARCHAR(100) NOT NULL,
    duration_minutes INT NOT NULL,

    CONSTRAINT chk_flight_number_not_empty
        CHECK (length(btrim(flight_number)) > 0),

    CONSTRAINT chk_departure_city_not_empty
        CHECK (length(btrim(departure_airport_city)) > 0),

    CONSTRAINT chk_arrival_city_not_empty
        CHECK (length(btrim(arrival_airport_city)) > 0),

    CONSTRAINT chk_duration_positive 
        CHECK (duration_minutes > 0),

    CONSTRAINT chk_different_cities 
        CHECK (lower(btrim(departure_airport_city)) <> lower(btrim(arrival_airport_city)))
);

CREATE OR REPLACE FUNCTION validate_flight_departure_date()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.departure_datetime::date < CURRENT_DATE
       OR NEW.departure_datetime::date > CURRENT_DATE + INTERVAL '7 days' THEN
        RAISE EXCEPTION 'Flight date must be within the next 7 days.'
            USING ERRCODE = '23514';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_validate_flight_departure_date
BEFORE INSERT OR UPDATE ON flights
FOR EACH ROW
EXECUTE FUNCTION validate_flight_departure_date();

CREATE INDEX idx_flights_departure_date
ON flights (departure_datetime);

CREATE INDEX idx_flights_departure_city_date
ON flights (lower(btrim(departure_airport_city)), departure_datetime);

CREATE INDEX idx_flights_arrival_city_date
ON flights (lower(btrim(arrival_airport_city)), departure_datetime);
