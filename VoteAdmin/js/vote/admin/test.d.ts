interface IAudible {
    isPlaying: boolean;
    turnOn(): void;
    playSelection(preset: number): void;
    turnOff(): void;
}
declare class CDPlayer implements IAudible {
    isPlaying: boolean;
    constructor();
    turnOn(): void;
    playSelection(preset: number): void;
    turnOff(): void;
    eject(): void;
}
declare class Radio implements IAudible {
    isPlaying: boolean;
    constructor();
    turnOn(): void;
    playSelection(preset: number): void;
    turnOff(): void;
}
declare class Vehicle {
    private _gasMileage;
    private _fuelAvailable;
    private _milesTraveled;
    private _moving;
    name: string;
    soundSystem: IAudible;
    constructor(mpg?: number, fuel?: number);
    useAccessory(): void;
    changeGear(): void;
    drive(): void;
    getGasMileage(): number;
    setGasMileage(mpg: number): void;
    getFuelAvailable(): number;
    setFuelAvailable(fuel: number): void;
    getMilesTraveled(): number;
}
declare class TireType {
    static SNOW: string;
    static HIGH_PERFORMANCE: string;
    static ECONOMICAL: string;
}
declare class Tire {
    private _type;
    constructor(tire: string);
    getType(): string;
    setType(tire: string): void;
}
declare class Car extends Vehicle {
    private _tires;
    constructor(mpg: number, fuel: number);
    useAccessory(): void;
    private openSunroof();
}
declare class Truck extends Vehicle {
    private _tires;
    constructor(mpg: number, fuel: number);
    useAccessory(): void;
    private lowerTailgate();
}
declare class Main {
    private _compact;
    private _pickup;
    constructor();
}
