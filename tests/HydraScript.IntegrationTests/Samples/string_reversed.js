let str = "string"
let i = ~str - 1
let revStr: string
while (i >= 0) {
    revStr = revStr + str[i]
    i = i - 1
}
>>> revStr