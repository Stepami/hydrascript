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

>>>a