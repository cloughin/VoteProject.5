
class Test {

  public static stat = "abc";

  constructor(private name: string) {
  }

  public val(newval?: string): string {
    if (newval !== undefined)
      this.name = newval;
    return this.name;
  }
}