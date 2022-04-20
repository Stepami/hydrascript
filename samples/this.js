let obj = {
    num: 1;
    flag: true;
    str: "field";
    toString => (): string {
        let s = "object obj:\n"
        return s + (this as string)
    };
}

print(obj.toString())