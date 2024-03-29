CREATE TABLE IF NOT EXISTS Model (
    id SERIAL,
    ModelID UUID DEFAULT gen_random_uuid() PRIMARY KEY NOT NULL,
    DisplayName TEXT,
    LastModifiedTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS ModelItem (
	ID SERIAL,
	ModelItemID UUID DEFAULT gen_random_uuid() PRIMARY KEY NOT NULL,
	DisplayName TEXT,
	HierachyIndex INT,
	ParentHierachyIndex INT,
	Color JSONB,
	Mesh JSONB,
	Matrix REAL[],
    Properties TEXT,
	LastModifiedTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	ModelID UUID REFERENCES Model(ModelID) NOT NULL,
    BatchedModelItemID UUID REFERENCES ModelItem(ModelItemID) DEFAULT NULL,
	FeatureID INTEGER DEFAULT NULL,
	GlbIndex INTEGER DEFAULT NULL
);