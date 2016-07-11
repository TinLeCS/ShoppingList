	var validFilesTypes = ["BMP", "GIF", "PNG", "JPG", "JPEG"];

	function CheckExtension(file) {
		/*global document: false */
		var filePath = file.value;
		var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toUpperCase();
		var isValidFile = false;

		for (var i = 0; i < validFilesTypes.length; i++) {
			if (ext == validFilesTypes[i]) {
				isValidFile = true;
				break;
			}
		}

		if (!isValidFile) {
			file.value = null;
			alert("Invalid Image File. You are only allowed to upload a " + validFilesTypes.join(", ") + " file.");
		}

		return isValidFile;
	}

	var validFileSize = 4 * 1024 * 1024;

	function CheckFileSize(file) {
		/*global document: false */
		var fileSize = file.files[0].size;
		var isValidFile = false;
		if (fileSize !== 0 && fileSize <= validFileSize) {
			isValidFile = true;
		}
		else {
			file.value = null;
			alert("The image size cannot exceed 4 MB and cannot be 0 MB.");
		}
		return isValidFile;
	}

	function CheckFile(file) {
		var isValidFile = CheckExtension(file);

		if (isValidFile)
			isValidFile = CheckFileSize(file);

		return isValidFile;
	}
