import * as $protobuf from "protobufjs";

/**
 * Callback as used by {@link util.asPromise}.
 * @param error Error, if any
 * @param params Additional arguments
 */
type asPromiseCallback = (error: (Error|null), ...params: any[]) => void;

/**
 * Appends code to the function's body or finishes generation.
 * @param [formatStringOrScope] Format string or, to finish the function, an object of additional scope variables, if any
 * @param [formatParams] Format parameters
 * @returns Itself or the generated function if finished
 * @throws {Error} If format parameter counts do not match
 */
type Codegen = (formatStringOrScope?: (string|{ [k: string]: any }), ...formatParams: any[]) => (Codegen|Function);

/**
 * Node-style callback as used by {@link util.fetch}.
 * @param error Error, if any, otherwise `null`
 * @param [contents] File contents, if there hasn't been an error
 */
type FetchCallback = (error: Error, contents?: string) => void;

/** Options as used by {@link util.fetch}. */
type FetchOptions = {

    /** Whether expecting a binary response */
    binary?: boolean;

    /** If `true`, forces the use of XMLHttpRequest */
    let xhr?: boolean;
};

/**
 * An allocator as used by {@link util.pool}.
 * @param size Buffer size
 * @returns Buffer
 */
type PoolAllocator = (size: number) => Uint8Array;

/**
 * A slicer as used by {@link util.pool}.
 * @param start Start offset
 * @param end End offset
 * @returns Buffer slice
 */
type PoolSlicer = (this: Uint8Array, start: number, end: number) => Uint8Array;

/** Enum descriptor. */
export interface IEnum {

    /** Enum values */
    values: { [k: string]: number };

    /** Enum options */
    options?: { [k: string]: any };
}

/** Reflected message field. */
export class Field extends FieldBase {

    /**
     * Constructs a new message field instance. Note that {@link MapField|map fields} have their own class.
     * @param name Unique name within its namespace
     * @param id Unique id within its namespace
     * @param type Value type
     * @param [rule="optional"] Field rule
     * @param [extend] Extended type if different from parent
     * @param [options] Declared options
     */
    constructor(name: string, id: number, type: string, rule?: (string|{ [k: string]: any }), extend?: (string|{ [k: string]: any }), options?: { [k: string]: any });

    /**
     * Constructs a field from a field descriptor.
     * @param name Field name
     * @param json Field descriptor
     * @returns Created field
     * @throws {TypeError} If arguments are invalid
     */
    public static fromJSON(name: string, json: IField): Field;

    /** Determines whether this field is packed. Only relevant when repeated and working with proto2. */
    public readonly packed: boolean;

    /**
     * Field decorator (TypeScript).
     * @param fieldId Field id
     * @param fieldType Field type
     * @param [fieldRule="optional"] Field rule
     * @param [defaultValue] Default value
     * @returns Decorator function
     */
    public static d<T extends number | number[] | Long | Long[] | string | string[] | boolean | boolean[] | Uint8Array | Uint8Array[] | Buffer | Buffer[]>(fieldId: number, fieldType: ("double"|"float"|"int32"|"uint32"|"sint32"|"fixed32"|"sfixed32"|"int64"|"uint64"|"sint64"|"fixed64"|"sfixed64"|"string"|"bool"|"bytes"|object), fieldRule?: ("optional"|"required"|"repeated"), defaultValue?: T): FieldDecorator;

    /**
     * Field decorator (TypeScript).
     * @param fieldId Field id
     * @param fieldType Field type
     * @param [fieldRule="optional"] Field rule
     * @returns Decorator function
     */
    public static d<T extends Message<T>>(fieldId: number, fieldType: (Constructor<T>|string), fieldRule?: ("optional"|"required"|"repeated")): FieldDecorator;
}

/** Base class of all reflected message fields. This is not an actual class but here for the sake of having consistent type definitions. */
export class FieldBase extends ReflectionObject {

    /**
     * Not an actual constructor. Use {@link Field} instead.
     * @param name Unique name within its namespace
     * @param id Unique id within its namespace
     * @param type Value type
     * @param [rule="optional"] Field rule
     * @param [extend] Extended type if different from parent
     * @param [options] Declared options
     * @param [comment] Comment associated with this field
     */
    constructor(name: string, id: number, type: string, rule?: (string|{ [k: string]: any }), extend?: (string|{ [k: string]: any }), options?: { [k: string]: any }, comment?: string);

    /** Field rule, if any. */
    public rule?: string;

    /** Field type. */
    public type: string;

    /** Unique field id. */
    public id: number;

    /** Extended type if different from parent. */
    public extend?: string;

    /** Whether this field is required. */
    public required: boolean;

    /** Whether this field is optional. */
    public optional: boolean;

    /** Whether this field is repeated. */
    public repeated: boolean;

    /** Whether this field is a map or not. */
    public map: boolean;

    /** Message this field belongs to. */
    public message: (Type|null);

    /** OneOf this field belongs to, if any, */
    public partOf: (OneOf|null);

    /** The field type's default value. */
    public typeDefault: any;

    /** The field's default value on prototypes. */
    public defaultValue: any;

    /** Whether this field's value should be treated as a long. */
    public long: boolean;

    /** Whether this field's value is a buffer. */
    public bytes: boolean;

    /** Resolved type if not a basic type. */
    public resolvedType: (Type|Enum|null);

    /** Sister-field within the extended type if a declaring extension field. */
    public extensionField: (Field|null);

    /** Sister-field within the declaring namespace if an extended field. */
    public declaringField: (Field|null);

    /** Comment for this field. */
    public comment: (string|null);

    /** undefined */
    public setOption(): void;

    /**
     * Converts this field to a field descriptor.
     * @param [toJSONOptions] JSON conversion options
     * @returns Field descriptor
     */
    public toJSON(toJSONOptions?: IToJSONOptions): IField;

    /**
     * Resolves this field's type references.
     * @returns `this`
     * @throws {Error} If any reference cannot be resolved
     */
    public resolve(): Field;
}

/** Field descriptor. */
export interface IField {

    /** Field rule */
    rule?: string;

    /** Field type */
    type: string;

    /** Field id */
    id: number;

    /** Field options */
    options?: { [k: string]: any };
}

/** Extension field descriptor. */
export interface IExtensionField extends IField {

    /** Extended type */
    extend: string;
}

/**
 * Decorator function as returned by {@link Field.d} and {@link MapField.d} (TypeScript).
 * @param prototype Target prototype
 * @param fieldName Field name
 */
type FieldDecorator = (prototype: object, fieldName: string) => void;

/**
 * A node-style callback as used by {@link load} and {@link Root#load}.
 * @param error Error, if any, otherwise `null`
 * @param [root] Root, if there hasn't been an error
 */
type LoadCallback = (error: (Error|null), root?: Root) => void;

/**
 * Loads one or multiple .proto or preprocessed .json files into a common root namespace and calls the callback.
 * @param filename One or multiple files to load
 * @param callback Callback function
 * @see {@link Root#load}
 */
export function load(filename: (string|string[]), callback: LoadCallback): void;

/**
 * Loads one or multiple .proto or preprocessed .json files into a common root namespace and returns a promise.
 * @param filename One or multiple files to load
 * @param [root] Root namespace, defaults to create a new one if omitted.
 * @returns Promise
 * @see {@link Root#load}
 */
export function load(filename: (string|string[]), root?: Root): Promise<Root>;

/** Build type, one of `"full"`, `"light"` or `"minimal"`. */
export const build: string;

/** Map field descriptor. */
export interface IMapField extends IField {

    /** Key type */
    keyType: string;
}

/** Extension map field descriptor. */
export interface IExtensionMapField extends IMapField {

    /** Extended type */
    extend: string;
}

/** Method descriptor. */
export interface IMethod {

    /** Method type */
    type?: string;

    /** Request type */
    requestType: string;

    /** Response type */
    responseType: string;

    /** Whether requests are streamed */
    requestStream?: boolean;

    /** Whether responses are streamed */
    responseStream?: boolean;

    /** Method options */
    options?: { [k: string]: any };
}

/** Reflected namespace. */
export class Namespace extends NamespaceBase {

    /**
     * Constructs a new namespace instance.
     * @param name Namespace name
     * @param [options] Declared options
     */
    constructor(name: string, options?: { [k: string]: any });

    /**
     * Constructs a namespace from JSON.
     * @param name Namespace name
     * @param json JSON object
     * @returns Created namespace
     * @throws {TypeError} If arguments are invalid
     */
    public static fromJSON(name: string, json: { [k: string]: any }): Namespace;

    /**
     * Converts an array of reflection objects to JSON.
     * @param array Object array
     * @param [toJSONOptions] JSON conversion options
     * @returns JSON object or `undefined` when array is empty
     */
    public static arrayToJSON(array: ReflectionObject[], toJSONOptions?: IToJSONOptions): ({ [k: string]: any }|undefined);

    /**
     * Tests if the specified id is reserved.
     * @param reserved Array of reserved ranges and names
     * @param id Id to test
     * @returns `true` if reserved, otherwise `false`
     */
    public static isReservedId(reserved: ((number[]|string)[]|undefined), id: number): boolean;

    /**
     * Tests if the specified name is reserved.
     * @param reserved Array of reserved ranges and names
     * @param name Name to test
     * @returns `true` if reserved, otherwise `false`
     */
    public static isReservedName(reserved: ((number[]|string)[]|undefined), name: string): boolean;
}

/** Base class of all reflection objects containing nested objects. This is not an actual class but here for the sake of having consistent type definitions. */
export abstract class NamespaceBase extends ReflectionObject {

    /** Nested objects by name. */
    public nested?: { [k: string]: ReflectionObject };

    /** Nested objects of this namespace as an array for iteration. */
    public readonly nestedArray: ReflectionObject[];

    /**
     * Converts this namespace to a namespace descriptor.
     * @param [toJSONOptions] JSON conversion options
     * @returns Namespace descriptor
     */
    public toJSON(toJSONOptions?: IToJSONOptions): INamespace;

    /**
     * Adds nested objects to this namespace from nested object descriptors.
     * @param nestedJson Any nested object descriptors
     * @returns `this`
     */
    public addJSON(nestedJson: { [k: string]: AnyNestedObject }): Namespace;

    /**
     * Gets the nested object of the specified name.
     * @param name Nested object name
     * @returns The reflection object or `null` if it doesn't exist
     */
    public get(name: string): (ReflectionObject|null);

    /**
     * Gets the values of the nested {@link Enum|enum} of the specified name.
     * This methods differs from {@link Namespace#get|get} in that it returns an enum's values directly and throws instead of returning `null`.
     * @param name Nested enum name
     * @returns Enum values
     * @throws {Error} If there is no such enum
     */
    public getEnum(name: string): { [k: string]: number };

    /**
     * Adds a nested object to this namespace.
     * @param object Nested object to add
     * @returns `this`
     * @throws {TypeError} If arguments are invalid
     * @throws {Error} If there is already a nested object with this name
     */
    public add(object: ReflectionObject): Namespace;

    /**
     * Removes a nested object from this namespace.
     * @param object Nested object to remove
     * @returns `this`
     * @throws {TypeError} If arguments are invalid
     * @throws {Error} If `object` is not a member of this namespace
     */
    public remove(object: ReflectionObject): Namespace;

    /**
     * Defines additial namespaces within this one if not yet existing.
     * @param path Path to create
     * @param [json] Nested types to create from JSON
     * @returns Pointer to the last namespace created or `this` if path is empty
     */
    public define(path: (string|string[]), json?: any): Namespace;

    /**
     * Resolves this namespace's and all its nested objects' type references. Useful to validate a reflection tree, but comes at a cost.
     * @returns `this`
     */
    public resolveAll(): Namespace;

    /**
     * Recursively looks up the reflection object matching the specified path in the scope of this namespace.
     * @param path Path to look up
     * @param filterTypes Filter types, any combination of the constructors of `protobuf.Type`, `protobuf.Enum`, `protobuf.Service` etc.
     * @param [parentAlreadyChecked=false] If known, whether the parent has already been checked
     * @returns Looked up object or `null` if none could be found
     */
    public lookup(path: (string|string[]), filterTypes: (any|any[]), parentAlreadyChecked?: boolean): (ReflectionObject|null);

    /**
     * Looks up the reflection object at the specified path, relative to this namespace.
     * @param path Path to look up
     * @param [parentAlreadyChecked=false] Whether the parent has already been checked
     * @returns Looked up object or `null` if none could be found
     */
    public lookup(path: (string|string[]), parentAlreadyChecked?: boolean): (ReflectionObject|null);

    /**
     * Looks up the {@link Type|type} at the specified path, relative to this namespace.
     * Besides its signature, this methods differs from {@link Namespace#lookup|lookup} in that it throws instead of returning `null`.
     * @param path Path to look up
     * @returns Looked up type
     * @throws {Error} If `path` does not point to a type
     */
    public lookupType(path: (string|string[])): Type;

    /**
     * Looks up the values of the {@link Enum|enum} at the specified path, relative to this namespace.
     * Besides its signature, this methods differs from {@link Namespace#lookup|lookup} in that it throws instead of returning `null`.
     * @param path Path to look up
     * @returns Looked up enum
     * @throws {Error} If `path` does not point to an enum
     */
    public lookupEnum(path: (string|string[])): Enum;

    /**
     * Looks up the {@link Type|type} or {@link Enum|enum} at the specified path, relative to this namespace.
     * Besides its signature, this methods differs from {@link Namespace#lookup|lookup} in that it throws instead of returning `null`.
     * @param path Path to look up
     * @returns Looked up type or enum
     * @throws {Error} If `path` does not point to a type or enum
     */
    public lookupTypeOrEnum(path: (string|string[])): Type;

    /**
     * Looks up the {@link Service|service} at the specified path, relative to this namespace.
     * Besides its signature, this methods differs from {@link Namespace#lookup|lookup} in that it throws instead of returning `null`.
     * @param path Path to look up
     * @returns Looked up service
     * @throws {Error} If `path` does not point to a service
     */
    public lookupService(path: (string|string[])): Service;
}

/** Namespace descriptor. */
export interface INamespace {

    /** Namespace options */
    options?: { [k: string]: any };

    /** Nested object descriptors */
    nested?: { [k: string]: AnyNestedObject };
}

/** Any extension field descriptor. */
type AnyExtensionField = (IExtensionField|IExtensionMapField);

/** Any nested object descriptor. */
type AnyNestedObject = (IEnum|IType|IService|AnyExtensionField|INamespace);

/** Oneof descriptor. */
export interface IOneOf {

    /** Oneof field names */
    oneof: string[];

    /** Oneof options */
    options?: { [k: string]: any };
}

/**
 * Decorator function as returned by {@link OneOf.d} (TypeScript).
 * @param prototype Target prototype
 * @param oneofName OneOf name
 */
type OneOfDecorator = (prototype: object, oneofName: string) => void;

/**
 * Parses the given .proto source and returns an object with the parsed contents.
 * @param source Source contents
 * @param [options] Parse options. Defaults to {@link parse.defaults} when omitted.
 * @returns Parser result
 */
export function parse(source: string, options?: IParseOptions): IParserResult;

/** Result object returned from {@link parse}. */
export interface IParserResult {

    /** Package name, if declared */
    package: (string|undefined);

    /** Imports, if any */
    imports: (string[]|undefined);

    /** Weak imports, if any */
    weakImports: (string[]|undefined);

    /** Syntax, if specified (either `"proto2"` or `"proto3"`) */
    syntax: (string|undefined);

    /** Populated root instance */
    root: Root;
}

/** Options modifying the behavior of {@link parse}. */
export interface IParseOptions {

    /** Keeps field casing instead of converting to camel case */
    keepCase?: boolean;

    /** Recognize double-slash comments in addition to doc-block comments. */
    alternateCommentMode?: boolean;

    /** Use trailing comment when both leading comment and trailing comment exist. */
    preferTrailingComment?: boolean;
}

/** Options modifying the behavior of JSON serialization. */
export interface IToJSONOptions {

    /** Serializes comments. */
    keepComments?: boolean;
}

/**
 * Named roots.
 * This is where pbjs stores generated structures (the option `-r, --root` specifies a name).
 * Can also be used manually to make roots available accross modules.
 */
export let roots: { [k: string]: Root };

/**
 * RPC implementation passed to {@link Service#create} performing a service request on network level, i.e. by utilizing http requests or websockets.
 * @param method Reflected or static method being called
 * @param requestData Request data
 * @param callback Callback function
 */
type RPCImpl = (method: (Method|rpc.ServiceMethod<Message<{}>, Message<{}>>), requestData: Uint8Array, callback: RPCImplCallback) => void;

/**
 * Node-style callback as used by {@link RPCImpl}.
 * @param error Error, if any, otherwise `null`
 * @param [response] Response data or `null` to signal end of stream, if there hasn't been an error
 */
type RPCImplCallback = (error: (Error|null), response?: (Uint8Array|null)) => void;

/** Service descriptor. */
export interface IService extends INamespace {

    /** Method descriptors */
    methods: { [k: string]: IMethod };
}

/**
 * Gets the next token and advances.
 * @returns Next token or `null` on eof
 */
type TokenizerHandleNext = () => (string|null);

/**
 * Peeks for the next token.
 * @returns Next token or `null` on eof
 */
type TokenizerHandlePeek = () => (string|null);

/**
 * Pushes a token back to the stack.
 * @param token Token
 */
type TokenizerHandlePush = (token: string) => void;

/**
 * Skips the next token.
 * @param expected Expected token
 * @param [optional=false] If optional
 * @returns Whether the token matched
 * @throws {Error} If the token didn't match and is not optional
 */
type TokenizerHandleSkip = (expected: string, optional?: boolean) => boolean;

/**
 * Gets the comment on the previous line or, alternatively, the line comment on the specified line.
 * @param [line] Line number
 * @returns Comment text or `null` if none
 */
type TokenizerHandleCmnt = (line?: number) => (string|null);

/** Handle object returned from {@link tokenize}. */
export interface ITokenizerHandle {

    /** Gets the next token and advances (`null` on eof) */
    next: TokenizerHandleNext;

    /** Peeks for the next token (`null` on eof) */
    peek: TokenizerHandlePeek;

    /** Pushes a token back to the stack */
    push: TokenizerHandlePush;

    /** Skips a token, returns its presence and advances or, if non-optional and not present, throws */
    skip: TokenizerHandleSkip;

    /** Gets the comment on the previous line or the line comment on the specified line, if any */
    cmnt: TokenizerHandleCmnt;

    /** Current line number */
    line: number;
}

/** Message type descriptor. */
export interface IType extends INamespace {

    /** Oneof descriptors */
    oneofs?: { [k: string]: IOneOf };

    /** Field descriptors */
    fields: { [k: string]: IField };

    /** Extension ranges */
    extensions?: number[][];

    /** Reserved ranges */
    reserved?: number[][];

    /** Whether a legacy group or not */
    group?: boolean;
}

/** Conversion options as used by {@link Type#toObject} and {@link Message.toObject}. */
export interface IConversionOptions {

    /**
     * Long conversion type.
     * Valid values are `String` and `Number` (the global types).
     * Defaults to copy the present value, which is a possibly unsafe number without and a {@link Long} with a long library.
     */
    longs?: Function;

    /**
     * Enum value conversion type.
     * Only valid value is `String` (the global type).
     * Defaults to copy the present value, which is the numeric id.
     */
    enums?: Function;

    /**
     * Bytes value conversion type.
     * Valid values are `Array` and (a base64 encoded) `String` (the global types).
     * Defaults to copy the present value, which usually is a Buffer under node and an Uint8Array in the browser.
     */
    bytes?: Function;

    /** Also sets default values on the resulting object */
    defaults?: boolean;

    /** Sets empty arrays for missing repeated fields even if `defaults=false` */
    arrays?: boolean;

    /** Sets empty objects for missing map fields even if `defaults=false` */
    objects?: boolean;

    /** Includes virtual oneof properties set to the present field's name, if any */
    oneofs?: boolean;

    /** Performs additional JSON compatibility conversions, i.e. NaN and Infinity to strings */
    json?: boolean;
}

/**
 * Decorator function as returned by {@link Type.d} (TypeScript).
 * @param target Target constructor
 */
type TypeDecorator<T extends Message<T>> = (target: Constructor<T>) => void;

/**
 * Any compatible Buffer instance.
 * This is a minimal stand-alone definition of a Buffer instance. The actual type is that exported by node's typings.
 */
export interface Buffer extends Uint8Array {
}

/**
 * Any compatible Long instance.
 * This is a minimal stand-alone definition of a Long instance. The actual type is that exported by long.js.
 */
export interface Long {

    /** Low bits */
    low: number;

    /** High bits */
    high: number;

    /** Whether unsigned or not */
    unsigned: boolean;
}

/**
 * A OneOf getter as returned by {@link util.oneOfGetter}.
 * @returns Set field name, if any
 */
type OneOfGetter = () => (string|undefined);

/**
 * A OneOf setter as returned by {@link util.oneOfSetter}.
 * @param value Field name
 */
type OneOfSetter = (value: (string|undefined)) => void;

/**
 * From object converter part of an {@link IWrapper}.
 * @param object Plain object
 * @returns Message instance
 */
type WrapperFromObjectConverter = (this: Type, object: { [k: string]: any }) => Message<{}>;

/**
 * To object converter part of an {@link IWrapper}.
 * @param message Message instance
 * @param [options] Conversion options
 * @returns Plain object
 */
type WrapperToObjectConverter = (this: Type, message: Message<{}>, options?: IConversionOptions) => { [k: string]: any };

/** Common type wrapper part of {@link wrappers}. */
export interface IWrapper {

    /** From object converter */
    fromObject?: WrapperFromObjectConverter;

    /** To object converter */
    toObject?: WrapperToObjectConverter;
}
