import * as fs from "fs";

// Define input directory of policy files
const directoryPath = `${process.cwd()}/policies/`;

// Define RegEx patterns
const blockPattern = /@{((?:[^{}]|{(?:[^{}]|{(?:[^{}]|{[^{}]*})*})*})*)}/g;
const inlinePattern = /@\((?:[^()]+|\((?:[^()]+|\([^()]*\))*\))*\)/g;
const namedValuePattern = /({{)(.*?)(}})/g;

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
	let template = fs.readFileSync(`${process.cwd()}/src/templates/script.csx`, "utf8");

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

		let variables: string = "";
		let found;
		let scriptBody = match;
		while ((found = namedValuePattern.exec(match)) !== null) {
			if(variables === "") {
				variables += `\t// The following named values have been extracted from the script and replaced with variables\r\n\t// Please check the script to ensure the string begins with a $ sign for string interpolation\r\n`;
			}
			const variableName = `nv_${found[2].replace("-", "").trim()}`;
			if(variables.includes(variableName) === false) {
				variables += `\tstring ${variableName} = \"\"; // Named Value: ${found[2].trim()}\r\n`;
			}
			scriptBody = scriptBody.replace((found[1] + found[2] + found[3]), `{${variableName}}`);
		}

		const blockTemplate = template.replace("{0}", variables);

		fs.writeFileSync(`${output}/block-${(index + 1).toString().padStart(3, "0")}.csx`, blockTemplate.replace('return "{1}";', scriptBody));
	});

	// Write the inline snippets out as C# script files
	inline.forEach((match, index) => {

		let variables: string = "";
		let found;
		let scriptBody = match;
		while ((found = namedValuePattern.exec(match)) !== null) {
			if(variables === "") {
				variables += `\t// The following named values have been extracted from the script and replaced with variables\r\n\t// Please check the script to ensure the string begins with a $ sign for string interpolation\r\n`;
			}
			const variableName = `nv_${found[2].replace("-", "").trim()}`;
			if(variables.includes(variableName) === false) {
				variables += `\tstring ${variableName} = \"\"; // Named Value: ${found[2].trim()}\r\n`;
			}
			scriptBody = scriptBody.replace((found[1] + found[2] + found[3]), `{${variableName}}`);
		}

		const inlineTemplate = template.replace("{0}", variables);

		fs.writeFileSync(`${output}/inline-${(index + 1).toString().padStart(3, "0")}.csx`, inlineTemplate.replace('"{1}"', scriptBody));
	});
};

extract();