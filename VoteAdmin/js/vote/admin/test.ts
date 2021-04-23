interface IAudible {
  isPlaying: boolean;
  turnOn(): void;
  playSelection(preset: number): void;
  turnOff(): void;
}

class CDPlayer implements IAudible {

  public isPlaying: boolean;

  constructor() {
    this.isPlaying = false;
  }

  public turnOn(): void {
    console.log("cd player on");
    this.isPlaying = true;
  }

  public playSelection(preset: number): void {
    console.log("cd player selection play: track", preset);
  }

  public turnOff(): void {
    console.log("cd player off");
    this.isPlaying = false;
  }

  public eject(): void {
    console.log("cd player eject");
  }

}

class Radio implements IAudible {

  public isPlaying: boolean;

  constructor() {
    this.isPlaying = false;
  }

  public turnOn(): void {
    console.log("radio on");
    this.isPlaying = false;
  }

  public playSelection(preset: number): void {
    console.log("radio selection play: channel preset", preset);
  }

  public turnOff(): void {
    console.log("radio off");
    this.isPlaying = false;
  }

}

class Vehicle {

  private _gasMileage: number;
  private _fuelAvailable: number;
  private _milesTraveled: number;
  private _moving: boolean = false;

  public name: string;
  public soundSystem: IAudible;

  constructor(mpg: number = 21, fuel: number = 18.5) {
    this._gasMileage = mpg;
    this._fuelAvailable = fuel;
    this._milesTraveled = 0;
  }

  public useAccessory(): void {
    console.log(this.name, "turned on lights");
  }

  public changeGear(): void {
    console.log(this.name, "changed gear");
  }

  public drive(): void {
    this._fuelAvailable--;
    this._milesTraveled += this._gasMileage;

    if (this._fuelAvailable > 0) {
      this.drive();
    } else {
      console.log("The", this.name, "has a gas mileage of", this._gasMileage,
        "and traveled", this._milesTraveled, "miles.");
    }
  }

  public getGasMileage(): number {
    return this._gasMileage;
  }

  public setGasMileage(mpg: number): void {
    this._gasMileage = mpg;
  }

  public getFuelAvailable(): number {
    return this._fuelAvailable;
  }

  public setFuelAvailable(fuel: number): void {
    this._fuelAvailable = fuel;
  }

  public getMilesTraveled(): number {
    return this._milesTraveled;
  }

}

class TireType {

  static SNOW: string = "snow";
  static HIGH_PERFORMANCE: string = "highPerformance";
  static ECONOMICAL: string = "economical";

}

class Tire {

  private _type: string;

  constructor(tire: string) {
    switch (tire) {
    case TireType.SNOW:
      this._type = "storm-ready snow";
      break;
    case TireType.HIGH_PERFORMANCE:
      this._type = "high-performance radial";
      break;
    case TireType.ECONOMICAL:
    default:
      this._type = "economical bias-ply";
    }
  }

  public getType(): string {
    return this._type;
  }

  public setType(tire: string): void {
    this._type = tire;
  }

}

class Car extends Vehicle {

  private _tires: Tire;

  constructor(mpg: number, fuel: number) {
    super(mpg, fuel);

    this.name = "Car";

    this._tires = new Tire(TireType.HIGH_PERFORMANCE);
    this.soundSystem = new CDPlayer();

    console.log(this.name, "has", this._tires.getType(), "tires");
  }

  public useAccessory(): void {
    this.openSunroof();
  }

  private openSunroof(): void {
    console.log(this.name, "opened sunroof");
  }

}

class Truck extends Vehicle {

  private _tires: Tire;

  constructor(mpg: number, fuel: number) {
    super(mpg, fuel);

    this.name = "Truck";

    this._tires = new Tire(TireType.SNOW);
    this.soundSystem = new Radio();

    console.log(this.name, "has", this._tires.getType(), "tires");
  }

  public useAccessory(): void {
    super.useAccessory();
    this.lowerTailgate();
  }

  private lowerTailgate(): void {
    console.log(this.name, "lowered tailgate");
  }

}

class Main {

  private _compact: Car;
  private _pickup: Truck;

  constructor() {
    this._compact = new Car(21, 18);
    this._compact.changeGear();
    this._compact.useAccessory();

    this._pickup = new Truck(16, 23);
    this._pickup.changeGear();
    this._pickup.useAccessory();

    this._compact.soundSystem.turnOn();
    this._compact.soundSystem.playSelection(2);
    this._pickup.soundSystem.turnOn();

    this._compact.drive();
    this._pickup.drive();
  }

}

