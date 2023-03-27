import * as fs from "fs";
import { extract } from "./extract-csharp-expressions";

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
			extract(directoryPath, file);
		}
	});
});