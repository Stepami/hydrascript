let summator = {
    x: 1;
    y: 2;
    sum => (): number {
        return x + y
    };
}

let s = summator.sum()
print(s as string)