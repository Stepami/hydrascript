type settable = {
    prop: string;
    setprop: (string) => void;
}

let obj: settable = {
    prop: "prop";
    setprop => (str: string) {
        prop = str
        if (prop == "1") {
            print("prop is one")
        }
    };
}

obj.setprop("1")
obj.setprop("2")
print(obj as string)