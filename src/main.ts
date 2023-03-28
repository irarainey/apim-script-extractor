import * as fs from "fs";

// Define input directory of policy files
const directoryPath = `${process.cwd()}/policies/`;

// Define RegEx patterns
const blockPattern = /@{((?:[^{}]|{(?:[^{}]|{(?:[^{}]|{[^{}]*})*})*})*)}/g;
const inlinePattern = /@\((?:[^()]+|\((?:[^()]+|\([^()]*\))*\))*\)/g;

export const extract = () => {
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
				extractScript(directoryPath, file);
			}
		});
	});
};

// Define the function to extract C# expressions from a policy file
const extractScript = (path: string, file: string) => {
	// Read the policy file
	const policy = fs.readFileSync(`${path}${file}`, "utf8");

	// Find all the C# expressions in the policy file as script blocks
	const blocks = Array.from(policy.matchAll(blockPattern), (m) => m[1] || m[2]);

	// Find all the C# expressions in the policy file as inline expressions
	const inline = policy.match(inlinePattern)?.map((m) => m.slice(2, -1)) || [];

	// Read the template file
	const template = fs.readFileSync(`${process.cwd()}/src/templates/script.csx`, "utf8");

	// Define the output directory name
	const output = `${path.replace("policies/", "")}scripts/${file.replace(".xml", "")}`;

	// Create the output directory
	fs.mkdirSync(output, { recursive: true });

	// Copy the context class into the output directory
	fs.copyFileSync(`${process.cwd()}/src/templates/context.csx`, `${output}/context.csx`);

	// Copy the context settings into the output directory
	fs.copyFileSync(`${process.cwd()}/src/templates/context.json`, `${output}/context.json`);

	// Write the block snippets out as C# script files
	blocks.forEach((match, index) => {
		fs.writeFileSync(`${output}/block-${(index + 1).toString().padStart(3, "0")}.csx`, template.replace('return "{0}";', match));
	});

	// Write the inline snippets out as C# script files
	inline.forEach((match, index) => {
		fs.writeFileSync(`${output}/inline-${(index + 1).toString().padStart(3, "0")}.csx`, template.replace('"{0}"', match));
	});
};
