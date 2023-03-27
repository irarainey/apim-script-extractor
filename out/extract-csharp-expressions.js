"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.extract = void 0;
const fs = require("fs");
// Define RegEx patterns
const bracePattern = /@{((?:[^{}]|{(?:[^{}]|{(?:[^{}]|{[^{}]*})*})*})*)}/g;
const bracketPattern = /@\((?:[^()]+|\((?:[^()]+|\([^()]*\))*\))*\)/g;
const extract = (directoryPath, filename) => {
    // Read the policy file
    const xmlFile = fs.readFileSync(`${directoryPath}${filename}`, "utf8");
    // Find all the C# expressions in the policy file as blocks
    const braceMatches = Array.from(xmlFile.matchAll(bracePattern), (m) => m[1] || m[2]);
    // Find all the C# expressions in the policy file as inline expressions
    const bracketMatches = xmlFile.match(bracketPattern)?.map((m) => m.slice(2, -1)) || [];
    // Read the template file
    const template = fs.readFileSync(`${process.cwd()}/src/templates/script.csx`, "utf8");
    // Define the output directory name
    const outputDirectory = filename.replace(".xml", "");
    // Create the output directory
    fs.mkdir(`${directoryPath}/scripts/${outputDirectory}`, { recursive: true }, (err) => {
        if (err) {
            console.error(err);
            return;
        }
    });
    // Copy the context class into the output directory
    fs.copyFile(`${process.cwd()}/src/templates/_context.csx`, `${directoryPath}/scripts/${outputDirectory}/_context.csx`, (err) => {
        if (err) {
            console.error(err);
            return;
        }
    });
    // Write the snippets out as C# scripts
    braceMatches.forEach((match, index) => {
        fs.writeFile(`${directoryPath}/scripts/${outputDirectory}/block-${(index + 1).toString().padStart(3, "0")}.csx`, template.replace("return \"{0}\";", match), (err) => {
            if (err) {
                console.error(err);
            }
        });
    });
    // Write the snippets out as C# scripts
    bracketMatches.forEach((match, index) => {
        fs.writeFile(`${directoryPath}/scripts/${outputDirectory}/inline-${(index + 1).toString().padStart(3, "0")}.csx`, template.replace("\"{0}\"", match), (err) => {
            if (err) {
                console.error(err);
            }
        });
    });
};
exports.extract = extract;
//# sourceMappingURL=extract-csharp-expressions.js.map