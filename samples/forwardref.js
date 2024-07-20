type A = {
    b: B;
}

type B = {
    a: A;
}

let a: A = {
    b: {
        a: null;
    };
}

print(a as string)