import * as fs from "fs";

// Define RegEx patterns
const blockPattern = /@{((?:[^{}]|{(?:[^{}]|{(?:[^{}]|{[^{}]*})*})*})*)}/g;
const inlinePattern = /@\((?:[^()]+|\((?:[^()]+|\([^()]*\))*\))*\)/g;

// Define the function to extract C# expressions from a policy file
export const extract = (path: string, file: string) => {
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
	fs.mkdir(output, { recursive: true }, (err) => {
		if (err) {
			console.error(err);
			return;
		}
	});
	
	// Copy the context class into the output directory
	fs.copyFile(`${process.cwd()}/src/templates/context.csx`, `${output}/context.csx`, (err) => {
		if (err) {
			console.error(err);
			return;
		}
	});

	// Copy the context settings into the output directory
	fs.copyFile(`${process.cwd()}/src/templates/context.json`, `${output}/context.json`, (err) => {
		if (err) {
			console.error(err);
			return;
		}
	});

	// Write the block snippets out as C# script files
	blocks.forEach((match, index) => {
		fs.writeFile(
			`${output}/block-${(index + 1).toString().padStart(3, "0")}.csx`,
			template.replace("return \"{0}\";", match),
			(err) => {
				if (err) {
					console.error(err);
				}
			}
		);
	});

	// Write the inline snippets out as C# script files
	inline.forEach((match, index) => {
		fs.writeFile(
			`${output}/inline-${(index + 1).toString().padStart(3, "0")}.csx`,
			template.replace("\"{0}\"", match),
			(err) => {
				if (err) {
					console.error(err);
				}
			}
		);
	});
};
