"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const fs = require("fs");
const extract_csharp_expressions_1 = require("./extract-csharp-expressions");
// Define input directory of policy files
const directoryPath = `${process.cwd()}/policies/`;
// Read all files in the directory
fs.readdir(directoryPath, (err, files) => {
    // Handle errors
    if (err) {
        console.log(`Error reading directory: ${err}`);
        return;
    }
    // Process each file
    files.forEach((file) => {
        if (file.endsWith(".xml") === true) {
            (0, extract_csharp_expressions_1.extract)(directoryPath, file);
        }
    });
});
//# sourceMappingURL=main.js.map